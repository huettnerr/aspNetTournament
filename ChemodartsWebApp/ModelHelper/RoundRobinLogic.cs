using Castle.Core.Internal;
using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace ChemodartsWebApp.ModelHelper
{
    public static class RoundRobinLogic
    {
        public static void UpdateSeedsInRound(Round? r)
        {
            if (r is null || r.Modus != RoundModus.RoundRobin) return;

            //Get all seeds
            List<Seed> allSeeds = new List<Seed>();
            r.Groups.OrderBy(g => g.GroupId).ToList().ForEach(g => allSeeds.AddRange(g.Seeds));

            //update seed numbers
            int seedNr = 1;
            allSeeds.ForEach(s => { s.SeedNr = seedNr; s.SeedName = $"Seed #{seedNr}"; seedNr++; });
        }

        private class SeedMatchCount
        {
            public Seed seed;
            public int cnt;

            public override string ToString()
            {
                return $"[{cnt}] {seed}";
            }
        }

        /// <summary>
        /// CAUTION: This function has been carefully debugged and testet.
        /// Even small changes will result in maybe completely unexspected behaviour.
        /// Edit with care or leave as is!!
        /// </summary>
        /// <param name="r"></param>
        public static void UpdateMatchOrderInRound(Round r)
        {
            if (r is null || r.Modus != RoundModus.RoundRobin) return;

            //Get all seeds
            List<Seed> allSeeds = new List<Seed>();
            r.Groups.OrderBy(g => g.GroupId).ToList().ForEach(g => allSeeds.AddRange(g.Seeds));

            //Get List of all matches in this round and reset match Order
            List<Match> allMatches = new List<Match>();
            r.Groups.ToList().ForEach(g => allMatches.AddRange(g.Matches.Where(m => m.Group.Round.Equals(r))));
            allMatches.ForEach(m => m.MatchOrderValue = null);

            //create container of matches per seed and sort them
            List<SeedMatchCount> seedMatchCountList = allSeeds
                .Select(s => new SeedMatchCount() { seed = s, cnt = s.Matches.Where(m => m.Group.Round.Equals(r)).Count() })
                .OrderByDescending(mps => mps.cnt)
                .ThenBy(mps => mps.seed.Matches.Min(m => m.MatchStage))
                .ToList();

            int matchOrder = 1;
            List<Seed> excludedSeeds = new List<Seed>();

            // run as long as at least one seed has more than one unordered match. Always start with highest match count (per seed)
            // the function tries to give each seed a break as long as possible between two matches
            for (int i = seedMatchCountList.Max(smc => smc.cnt); i > 0; i = seedMatchCountList.Max(smc => smc.cnt))
            {
                List<Match> tmpMatches = new List<Match>(); //matches in the current tier
                bool stopExcluding = false; //at some point it is necessary for a player to have multiple matches in one tier

                //Update in tiers of seeds with same remaining match count
                while (seedMatchCountList.Count(smc => smc.cnt == i) > 0)
                {
                    //exclude seeds which are already been picked
                    List<SeedMatchCount> tmpSMC = seedMatchCountList.Where(mps => mps.cnt == i && !excludedSeeds.Contains(mps.seed)).ToList();
                    if (tmpSMC.Count == 0)
                    {
                        //gradually remove seeds from excluded list when no more matches without seed duplication can be found
                        stopExcluding = true; // also stop excluding to prevent eternal loops
                        excludedSeeds.RemoveRange(0, Math.Min(1, excludedSeeds.Count));
                        continue;
                    }

                    //get one of the seeds with the highest matchCount
                    //SeedMatchCount? smc = tmpSMC.FirstOrDefault(mps => mps.cnt == i);
                    foreach (SeedMatchCount smc in tmpSMC)
                    {
                        if (smc is null) break;

                        //Get the first match of that seed that has no match order yet
                        Match? m = smc.seed.Matches.OrderBy(m => m.MatchStage).FirstOrDefault(m => m.MatchOrderValue is null);

                        //Make sure each match gets picked only once
                        if (m is object && allMatches.Contains(m))
                        {
                            //Make sure each seed gets picked only once in the same loop
                            int seedsOnEx = seedsOfMatchOnExclusionList(m, excludedSeeds);
                            if (seedsOnEx == 0)
                            {
                                stopExcluding = false;
                                SeedMatchCount? origSMC1 = seedMatchCountList.Find(x => x.seed.Equals(m.Seed1));
                                SeedMatchCount? origSMC2 = seedMatchCountList.Find(x => x.seed.Equals(m.Seed2));
                                if (origSMC1 is null || origSMC2 is null) return;

                                origSMC1.cnt -= 1;
                                origSMC2.cnt -= 1;
                                allMatches.Remove(m);
                                tmpMatches.Add(m);
                            }

                            //Exclude the players of the current mapped match for as long as possible
                            if (!stopExcluding)
                            {
                                if (!excludedSeeds.Contains(m.Seed1)) excludedSeeds.Add(m.Seed1);
                                if (!excludedSeeds.Contains(m.Seed2)) excludedSeeds.Add(m.Seed2);
                                break; // break the tmpSMC foreach loop and update it with new exclusions
                            }
                        }
                    }

                    if (tmpSMC.Count > 0 && stopExcluding)
                    {
                        // there is one match found, but all the opponents are still on the exclude list
                        // remove gradually till two available players are found
                        excludedSeeds.RemoveRange(0, Math.Min(1, excludedSeeds.Count));
                    }
                }

                //update the match order of the current tier and move to the next
                tmpMatches.ToList().ForEach(m => {
                    m.MatchOrderValue = matchOrder++;
                });
            }
        }

        private static int seedsOfMatchOnExclusionList(Match m, List<Seed> ex)
        {
            int cnt = 0;
            cnt += ex.Contains(m.Seed1) ? 1 : 0;
            cnt += ex.Contains(m.Seed2) ? 1 : 0;
            return cnt;
        }

        public static async Task RecreateAllMatches(ChemodartsContext context, Round? r)
        {
            //Delete all old matches
            IEnumerable<Match> oldMatches = await context.Matches.Where(m => m.Group.Round.RoundId == r.RoundId).ToListAsync();
            context.Matches.RemoveRange(oldMatches);

            List<Match> newMatches = new List<Match>();
            foreach (Group g in r.Groups) 
            {
                newMatches.AddRange(createMatches(g));
            }
            if (newMatches.IsNullOrEmpty()) return;

            context.Matches.AddRange(newMatches);
            await context.SaveChangesAsync();

            UpdateSeedsInRound(r);
            UpdateMatchOrderInRound(r);
            await context.SaveChangesAsync();
        }

        #region Group Creator

        public static async Task<bool> CreateGroup(ChemodartsContext context, GroupFactoryRR factory, Round r, ModelStateDictionary? modelState)
        {
            if (r is null || r.Modus != RoundModus.RoundRobin) return false;

            factory.R = r;
            Group? g = await context.Groups.CreateWithFactory(modelState, factory);
            if (g is null) return false;

            try
            {
                //Create the seeds
                List<Seed>? seeds = createSeeds(g, factory.PlayersPerGroup);
                if (seeds is null) throw new ArgumentNullException();

                context.Seeds.AddRange(seeds);
                await context.SaveChangesAsync(); //Needed so that the seeds get their id's

                //Map the seeds to the tournament
                List<MapRoundSeedPlayer>? mappers = createMapping(r, seeds);
                if (mappers is null) throw new ArgumentNullException();

                context.MapperRSP.AddRange(mappers);
                await context.SaveChangesAsync();   

                //create the matches
                List<Match> newMatches = createMatches(g);
                if(newMatches.IsNullOrEmpty()) throw new ArgumentNullException();

                context.Matches.AddRange(newMatches);
                await context.SaveChangesAsync();

                UpdateSeedsInRound(r);
                UpdateMatchOrderInRound(r);

                //Finally save to database
                await context.SaveChangesAsync();

                return true;
            }
            catch(ArgumentNullException e)
            {
                context.Remove(g);
                await context.SaveChangesAsync();
                return false;
            }
        }

        private static List<Seed>? createSeeds(Group? g, int cnt)
        {
            if (g is null) return null;

            SeedFactory sf = new SeedFactory("", null);
            sf.G = g;

            List<Seed> seeds = new List<Seed>();
            Seed? tmpSeed;
            for (int i = 0; i < cnt; i++)
            {
                tmpSeed = sf.Create();
                if (tmpSeed is null) return null;
                seeds.Add(tmpSeed);
            }

            return seeds;
        }

        private static List<MapRoundSeedPlayer>? createMapping(Round? r, List<Seed>? seeds)
        {
            if (r is null || seeds is null) return null;

            List<MapRoundSeedPlayer> mtsps = new List<MapRoundSeedPlayer>();
            foreach (Seed seed in seeds)
            {
                mtsps.Add(new MapRoundSeedPlayer()
                {
                    TSP_SeedId = seed.SeedId,
                    TSP_RoundId = r.RoundId,
                    TSP_PlayerCheckedIn = false,
                    TSP_PlayerFixed = false,
                    Player = null
                });
            }

            return mtsps;
        }

        #endregion

        #region Match Creator

        private static List<Match> createMatches(Group g)
        {
            List<Match> matches = new List<Match>();

            List<Match> tmpList = GenerateRoundRobinMatchesCircleMethod(g);

            tmpList.ForEach(m => {
                m.GroupId = g.GroupId;
            });
            matches.AddRange(tmpList);

            return matches;
        }

        static List<Match> GenerateRoundRobinMatchesCircleMethod(Group? g)
        {
            List<Match> matches = new List<Match>();

            if(g is null) return matches;

            List<Seed> seeds = g.Seeds.ToList();
            int numTeams = seeds.Count % 2 == 0 ? seeds.Count : seeds.Count + 1; 
            int totalStages = numTeams - 1;
            int halfTeams = numTeams / 2;

            List<int> seedIds = seeds.Select(s => s.SeedId).ToList();
            if (seeds.Count % 2 == 1) seedIds.Add(-1);

            for (int stage = 1; stage <= totalStages; stage++)
            {
                for (int i = 0; i < halfTeams; i++)
                {
                    int seed1Id = seedIds[i];
                    int seed2Id = seedIds[numTeams - 1 - i];

                    if (seed1Id >= 0 && seed2Id >= 0)
                    {
                        matches.Add(new Match() { 
                            GroupId = g.GroupId, 
                            Seed1Id = seed1Id, 
                            Seed2Id = seed2Id, 
                            MatchStage = stage
                        });
                    }
                }

                // Rotate the teams (except the first one)
                int temp = seedIds[numTeams - 1];
                for (int i = numTeams - 1; i >= 2; i--)
                {
                    seedIds[i] = seedIds[i - 1];
                }
                seedIds[1] = temp;
            }

            if (g.GroupHasRematch)
            {
                int matchCount = matches.Count;
                for (int i = 0; i < matchCount; i++)
                {
                    matches.Add(new Match() { 
                        GroupId = g.GroupId,
                        Seed1Id = matches[i].Seed2Id, 
                        Seed2Id = matches[i].Seed1Id, 
                        MatchStage = matches[i].MatchStage + totalStages
                    });
                }
            }

            return matches;
        }

        #endregion
    }
}
