using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ModelHelper
{
    public class ProgressionManager
    {
        private ChemodartsContext _context;

        public ProgressionManager(ChemodartsContext context) 
        { 
            _context= context;
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

        //NOTE TODO: Bye rounds are not supported yet
        public async Task<bool> ManageRanking(MapRoundProgression mrp, bool fixedPositions)
        {
            if(mrp.TargetRound?.Groups is null) return false;
            if(!(mrp.TargetRound.Modus == RoundModus.SingleKo || mrp.TargetRound.Modus == RoundModus.DoubleKo)) return false;

            int numberOfPlayers = mrp.AdvanceCount * (mrp.BaseRound.Groups?.Count ?? 0);

            if(!await RoundKoLogic.CreateSystemSingleKO(_context, mrp.TargetRound, numberOfPlayers, false))
            {
                return false;
            }

            List<Match>? firstRoundMatches = mrp.TargetRound.Groups.MinBy(g => g.GroupOrderValue)?.Matches.ToList();
            if (firstRoundMatches is null || firstRoundMatches.Count == 0) return false;

            //Get all the seeds that advance
            List<Seed>? relevantSeeds = getRelevantSeeds(mrp);
            if (relevantSeeds is null || relevantSeeds.Count == 0) return false;

            //Check if somewhere is an error
            if (2 * firstRoundMatches.Count > relevantSeeds.Count) return false;

            if (fixedPositions)
            {
                //Order the seeds first by rank and afterwards by the order of the groups
                relevantSeeds = relevantSeeds.OrderBy(s => s.SeedRank).ThenBy(s => s.Group.GroupOrderValue).ToList();

                int i = 0;
                foreach (Match m in firstRoundMatches)
                {
                    m.Seed1 = relevantSeeds.FirstOrDefault();
                    m.Seed2 = relevantSeeds.LastOrDefault();

                    relevantSeeds.Remove(m.Seed1);
                    relevantSeeds.Remove(m.Seed2);
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
    }
}
