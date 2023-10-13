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
            if (mrp.BaseRound?.Groups is null || mrp.BaseRound.Groups.Count == 0 || mrp.TargetRound is null)
            {
                ErrorMessage = $"No groups in Base round or target round is not set";
                return false;
            }

            if(!(mrp.TargetRound.Modus == RoundModus.SingleKo || mrp.TargetRound.Modus == RoundModus.DoubleKo))
            {
                ErrorMessage = $"Target Round is not configured for KO tournament brackets!";
                return false;
            }

            int numberOfGroups = mrp.BaseRound.Groups.Count;
            int numberOfPlayers = mrp.AdvanceCount * numberOfGroups;
            int numberOfByePlayers = mrp.ByeCount * numberOfGroups;

            if (!await RoundKoLogic.CreateKoSystem(_context, mrp.TargetRound, numberOfPlayers))
            {
                ErrorMessage = $"Failed to create Ko-System!";
                return false;
            }

            //Get all the seeds that advance
            List<Seed>? relevantSeeds;
            if (!_useDummySeeds)
            {
                //Get the advancing seeds from base round
                relevantSeeds = Ranking.GetProgressingSeeds(mrp.BaseRound, mrp.AdvanceCount);
                if(relevantSeeds is null || relevantSeeds.Count == 0)
                {
                    ErrorMessage = $"No Advancing Seeds found!";
                    return false;
                }

                //Order the seeds first by rank and afterwards by the order of the groups
                relevantSeeds = relevantSeeds.OrderBy(s => s.SeedRank).ThenBy(s => s.Group?.GroupOrderValue).ToList();
            }
            else
            {
                //Use dummy names
                Group? firstRoundGroup = RoundKoLogic.GetFirstRoundGroup(mrp.TargetRound);
                if(firstRoundGroup is null) return false;

                relevantSeeds = new List<Seed>();
                for (int rank = 1; rank <= mrp.AdvanceCount; rank++)
                {
                    List<Group> orderedGroups = mrp.BaseRound.Groups?.OrderBy(g => g.GroupOrderValue).ToList() ?? new List<Group>();
                    foreach (Group g in orderedGroups)
                    {
                        relevantSeeds.Add(new Seed()
                        {
                            Group = firstRoundGroup,
                            SeedName = $"{rank}. Gruppe \"{g.GroupName}\""
                        });
                    }
                }
            }

            // fill the seeds
            await RoundKoLogic.FillSeeds(
                _context, 
                fixedPositions ? RoundKoLogic.SeedingType.FixedForAll : RoundKoLogic.SeedingType.Random, 
                mrp.TargetRound,
                (_useDummySeeds && !fixedPositions) ? null : relevantSeeds //when random dummys, the group names shall not be used
            );

            //Get the matches of the first round of the tournament bracket
            List<Match>? firstRoundMatches = RoundKoLogic.GetFirstRoundMatches(mrp.TargetRound);
            if (firstRoundMatches is null || firstRoundMatches.Count == 0)
            {
                ErrorMessage = $"No first round matches found!";
                return false;
            }

            //move every second match in the other half of the bracket to ensure players of the same group meet as late as possible
            for (int i = 1; i < firstRoundMatches.Count / 2; i+=2)
            {
                int otherBracketHalfPosition = (i + firstRoundMatches.Count / 2) % firstRoundMatches.Count;

                //store other match's seeds
                Seed? tmpSeed1 = firstRoundMatches[otherBracketHalfPosition].Seed1;
                Seed? tmpSeed2 = firstRoundMatches[otherBracketHalfPosition].Seed2;

                //update other match's seeds
                firstRoundMatches[otherBracketHalfPosition].Seed1 = firstRoundMatches[i].Seed1;
                firstRoundMatches[otherBracketHalfPosition].Seed2 = firstRoundMatches[i].Seed2;

                //update match's seeds
                firstRoundMatches[i].Seed1 = tmpSeed1;
                firstRoundMatches[i].Seed2 = tmpSeed2;
            }


            //update the seeds of the following rounds to show valid bracket information
            RoundKoLogic.UpdateKoRoundSeeds(_context, mrp.TargetRound);

            return true;
#if false

            //Fill with bye's if necessary
            relevantSeeds = RoundKoLogic.FillWithByeSeeds(relevantSeeds, 2 * numberOfMatches);
            //Seed byeSeed = new Seed() { SeedName = "Bye" };
            //for (int i = 0; i < numberOfByePlayers; i++) relevantSeeds.Add(byeSeed);

            ////Check if tournament tree can be filled with the given information
            //if (2 * numberOfMatches != relevantSeeds.Count)
            //{
            //    //This is not the case. Probably because the number of seeds advancing doesn't fit the required ko stages
            //    ErrorMessage = $"Number of Advancing Seeds ({relevantSeeds.Count}) of the {mrp.BaseRound.Groups?.Count} groups can't be mapped to the required minimum of {firstRoundMatches.Count} matches. Check Progression Settings and Group Count";
            //    return false;
            //}

            //fill the matches
            if (fixedPositions)
            {
                //Pair the seeds top down so that seeds get matched with the lowest rank possible
                List<Tuple<int, int>>? pairsList = RoundKoLogic.GetSeedPairsOfBracket(numberOfMatches);
                if(pairsList is null) return false;

                //Fill matches based on List of pairs
                for (int i = 0; i < numberOfMatches; i++)
                {
                    //move every second match in the other half of the bracket to ensure players of the same group meet as late as possible
                    int pairListPosition = (i % 2 == 0) ? i : (i + numberOfMatches / 2) % numberOfMatches;

                    //fill match seeds according to list
                    Match m = firstRoundMatches[i];
                    m.Seed1 = relevantSeeds[pairsList[pairListPosition].Item1 - 1];
                    m.Seed2 = relevantSeeds[pairsList[pairListPosition].Item2 - 1];

                    //handle byes
                    if (m.Seed1.Equals(RoundKoLogic.BYE_SEED))
                    {
                        m.Seed1 = null;
                        m.SetNewStatus(Match.MatchStatus.Finished);
                    }
                    else if(m.Seed2.Equals(RoundKoLogic.BYE_SEED))
                    {
                        m.Seed2 = null;
                        m.SetNewStatus(Match.MatchStatus.Finished);
                    }
                }
            }
            else
            {
                //Randomize Seeds for KO-Round
                firstRoundMatches = RoundKoLogic.FillRandomizeSeedsForMatches(firstRoundMatches, relevantSeeds);
            }

            //save in database
            _context.Matches.UpdateRange(firstRoundMatches);
            await _context.SaveChangesAsync();

#endif
        }

        public async Task<bool> ManagePointsRanking(MapRoundProgression mrp)
        {
            if(mrp.TargetRound?.Modus != RoundModus.Ranking) return false;

            //TODO

            return false;
        }
    }
}
