using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ModelHelper
{
    public class ProgressionManager
    {
        public string ErrorMessage { get; private set; }

        private ChemodartsContext _context;
        private bool _useDummySeeds;

        public ProgressionManager(ChemodartsContext context, bool useDummySeeds = false) 
        { 
            _context= context;
            _useDummySeeds = useDummySeeds;
        }

        public async Task<bool> ManageAll(Round? r)
        {
            if (r is null) return false;

            bool overallSuccess = true;
            foreach(var mrp in r.ProgressionRulesAsBase)
            {
                overallSuccess &= await Manage(mrp);
            }

            return overallSuccess;  
        }

        public async Task<bool> Manage(MapRoundProgression? mrp)
        {
            if(mrp is null) return false;

            switch (mrp.ProgressionType)
            {
                case MapRoundProgression.TournamentProgressionType.PointsOnly:
                    return await ManagePointsRanking(mrp);
                case MapRoundProgression.TournamentProgressionType.RankingFixed:
                    return await ManageRanking(mrp, true);
                case MapRoundProgression.TournamentProgressionType.RankingRandom:
                    return await ManageRanking(mrp, false);
                default:
                    return false;
            }
        }

        public async Task<bool> ManageRanking(MapRoundProgression mrp, bool fixedPositions)
        {
            if(mrp.TargetRound?.Groups is null) return false;
            if(!(mrp.TargetRound.Modus == RoundModus.SingleKo || mrp.TargetRound.Modus == RoundModus.DoubleKo)) return false;

            int numberOfGroups = mrp.BaseRound.Groups?.Count ?? 0;
            int numberOfPlayers = mrp.AdvanceCount * numberOfGroups;
            int numberOfByePlayers = mrp.ByeCount * numberOfGroups;

            if(!await RoundKoLogic.CreateSystemSingleKO(_context, mrp.TargetRound, numberOfPlayers, false))
            {
                return false;
            }

            List<Match>? firstRoundMatches = mrp.TargetRound.Groups.MinBy(g => g.GroupOrderValue)?.Matches.ToList();
            if (firstRoundMatches is null || firstRoundMatches.Count == 0) return false;

            //Get all the seeds that advance
            List<Seed>? relevantSeeds;
            if (_useDummySeeds)
            {
                relevantSeeds = getRelevantSeeds(mrp);

                //Order the seeds first by rank and afterwards by the order of the groups
                relevantSeeds = relevantSeeds.OrderBy(s => s.SeedRank).ThenBy(s => s.Group?.GroupOrderValue).ToList();

                if (relevantSeeds is null || relevantSeeds.Count == 0) return false;
            }
            else
            {
                Group? firstRoundGroup = firstRoundMatches.FirstOrDefault()?.Group;
                if(firstRoundGroup is null) return false;

                relevantSeeds = new List<Seed>();
                int seedRank = 1;
                for (int rank = 1; rank <= mrp.AdvanceCount; rank++)
                {
                    List<Group> orderedGroups = mrp.BaseRound.Groups?.OrderBy(g => g.GroupOrderValue).ToList() ?? new List<Group>();
                    foreach (Group g in orderedGroups)
                    {
                        relevantSeeds.Add(new Seed()
                        {
                            Group = firstRoundGroup,
                            SeedName = $"{rank}. Gruppe \"{g.GroupName}\""
                            //SeedName = $"Seed {seedRank++}"
                        });
                    }
                }
            }

            //Fill with bye's if necessary
            Seed byeSeed = new Seed() { SeedName = "Bye" };
            for (int i = 0; i < numberOfByePlayers; i++) relevantSeeds.Add(byeSeed);

            //Check if tournament tree can be filled with the given information
            if (2 * firstRoundMatches.Count != relevantSeeds.Count)
            {
                //This is not the case. Probably because the number of seeds advancing doesn't fit the required ko stages
                ErrorMessage = $"Number of Advancing Seeds ({relevantSeeds.Count}) of the {mrp.BaseRound.Groups?.Count} groups can't be mapped to the required minimum of {firstRoundMatches.Count} matches. Check Progression Settings and Group Count";
                return false;
            }

            if (fixedPositions)
            {
                //Pair the seeds top down so that seeds get matched with the lowest rank possible
                List<Tuple<int, int>>? pairsList = getSeedPairs(firstRoundMatches.Count);
                if(pairsList is null) return false;

                //Fill matches based on List and reorder every second match
                int numberOfMatches = firstRoundMatches.Count;
                for (int i = 0; i < numberOfMatches; i++)
                {
                    //move every second match in the other half of the bracket to ensure players of the same group meet as late as possible
                    int pairListPosition = (i % 2 == 0) ? i : (i + numberOfMatches / 2) % numberOfMatches;

                    //fill match seeds according to list
                    Match m = firstRoundMatches[i];
                    m.Seed1 = relevantSeeds[pairsList[pairListPosition].Item1 - 1];
                    m.Seed2 = relevantSeeds[pairsList[pairListPosition].Item2 - 1];

                    //handle byes
                    if (m.Seed1.Equals(byeSeed))
                    {
                        m.Seed1 = null;
                        m.SetNewStatus(Match.MatchStatus.Finished);
                    }
                    else if(m.Seed2.Equals(byeSeed))
                    {
                        m.Seed2 = null;
                        m.SetNewStatus(Match.MatchStatus.Finished);
                    }
                }
            }
            else
            {
                //Randomize Seeds for KO-Round
                firstRoundMatches = RoundKoLogic.RandomizeSeedsForMatches(firstRoundMatches, relevantSeeds);
            }

            _context.Matches.UpdateRange(firstRoundMatches);
            await _context.SaveChangesAsync();

            RoundKoLogic.UpdateKoRoundSeeds(_context, mrp.TargetRound);

            return true;
        }

        private List<Tuple<int, int>>? getSeedPairs(int numberOfMatches) 
        {
            if (!(numberOfMatches > 0 && (numberOfMatches & (numberOfMatches - 1)) == 0))
            {
                ErrorMessage = $"Number of Matches ({numberOfMatches}) is no power of 2";
                return null;
            }

            //Create a List of the seed Ranks for the matches
            List<Tuple<int, int>> pairsList = new List<Tuple<int, int>>() { new Tuple<int, int>(1, 2) };
            int rankCntInStage = 0;
            while (pairsList.Count < numberOfMatches)
            {
                //Split the old pairs
                List<int> ranksInPreviousStage = new List<int>();
                pairsList.ForEach(pair =>
                {
                    ranksInPreviousStage.Add(pair.Item1);
                    ranksInPreviousStage.Add(pair.Item2);
                });
                rankCntInStage = 2 * ranksInPreviousStage.Count;

                //combine with adjecent rank
                pairsList = new List<Tuple<int, int>>();
                ranksInPreviousStage.ForEach(rank =>
                {
                    pairsList.Add(new Tuple<int, int>(rank, rankCntInStage - (rank - 1)));
                });
            }

            return pairsList;
        }

        //private List<Match>

        public async Task<bool> ManagePointsRanking(MapRoundProgression mrp)
        {
            if(mrp.TargetRound?.Modus != RoundModus.Ranking) return false;

            //TODO

            return false;
        }

        private List<Seed>? getRelevantSeeds(MapRoundProgression mrp)
        {

            List<Seed> seeds = new List<Seed>();
            switch (mrp.BaseRound.Modus)
            {
                case RoundModus.RoundRobin:
                    foreach (Group g in mrp.BaseRound.Groups)
                    {
                        seeds.AddRange(g.RankedSeeds.ToList().GetRange(0, mrp.AdvanceCount));
                    }
                    return seeds;
                case RoundModus.SingleKo:
                case RoundModus.DoubleKo:
                case RoundModus.Ranking:
                default:
                    return null;
            }
        }

        private List<Match> dummyfySeedNames(List<Match> matches)
        {
            foreach (Match match in matches)
            {
                match.Seed1 = new Seed()
                {
                    Group = match.Group,
                    SeedName = $"{match.Seed1?.SeedRank}. Gruppe \"{match.Seed1?.Group.GroupName}\""
                };
                match.Seed2 = new Seed()
                {
                    Group = match.Group,
                    SeedName = $"{match.Seed2?.SeedRank}. Gruppe \"{match.Seed2?.Group.GroupName}\""
                };
            }
            return matches;
        }
    }
}
