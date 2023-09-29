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

            //sort matches by matches per seed
            List<SeedMatchCount> matchesPerSeed = allSeeds
                .Select(s => new SeedMatchCount() { seed = s, cnt = s.Matches.Where(m => m.Group.Round.Equals(r)).Count() })
                .OrderByDescending(mps => mps.cnt)
                .ThenBy(mps => mps.seed.Matches.Min(m => m.MatchStage))
                .ToList();

            int matchOrder = 1;
            List<Seed> excludedSeeds = new List<Seed>();
            for (int i = matchesPerSeed.Max(mps => mps.cnt); i > 0; i = matchesPerSeed.Max(mps => mps.cnt))
            {
                List<Match> tmpMatches = new List<Match>();
                bool stopExcluding = false;
                //int excludeFromExcludeCnt = 1;
                while (matchesPerSeed.Count(mps => mps.cnt == i) > 0)
                {
                    //exclude seeds which are already been picked
                    List<SeedMatchCount> tmpSMC = matchesPerSeed.Where(mps => mps.cnt == i && !excludedSeeds.Contains(mps.seed)).ToList();
                    if (tmpSMC.Count == 0)
                    {
                        //gradually remove seeds from excluded list when no more matches without seed duplication can be found
                        stopExcluding = true;
                        excludedSeeds.RemoveRange(0, Math.Min(1, excludedSeeds.Count));
                        continue;
                    }

                    //get one seed with the highest matchCount
                    SeedMatchCount? smc = tmpSMC.FirstOrDefault(mps => mps.cnt == i);
                    if (smc is null) break;

                    //Get the first match of that seed that has no match order yet
                    Match? m = smc.seed.Matches.OrderBy(m => m.MatchStage).FirstOrDefault(m => m.MatchOrderValue is null);

                    //Make sure each match gets picked only once
                    if (m is object && allMatches.Contains(m))
                    {
                        //Make sure each seed gets picked only once in the same loop;
                        int seedsOnEx = seedsOfMatchOnExclusionList(m, excludedSeeds);
                        if (seedsOnEx == 0)
                        {
                            stopExcluding = false;
                            SeedMatchCount? origSMC1 = matchesPerSeed.Find(x => x.seed.Equals(m.Seed1));
                            SeedMatchCount? origSMC2 = matchesPerSeed.Find(x => x.seed.Equals(m.Seed2));
                            if (origSMC1 is null || origSMC2 is null) return;

                            origSMC1.cnt -= 1;
                            origSMC2.cnt -= 1;
                            allMatches.Remove(m);
                            tmpMatches.Add(m);
                        }
                        else if (stopExcluding && seedsOnEx == 1)
                        {
                            //To Prevent eternal loop remove 1 more
                            excludedSeeds.RemoveRange(0, Math.Min(1, excludedSeeds.Count));
                        }

                        if (!stopExcluding)
                        {
                            if (!excludedSeeds.Contains(m.Seed1)) excludedSeeds.Add(m.Seed1);
                            if (!excludedSeeds.Contains(m.Seed2)) excludedSeeds.Add(m.Seed2);
                        }
                    }
                }

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
            context.Matches.AddRange(newMatches);
            await context.SaveChangesAsync();
        }

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

                context.MapperRP.AddRange(mappers);
                await context.SaveChangesAsync();   

                UpdateSeedsInRound(r);
                UpdateMatchOrderInRound(r);

                //create the matches
                List<Match> newMatches = createMatches(g);
                context.Matches.AddRange(newMatches);

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

        #region Match Creator

        private static List<Match> createMatches(Group g)
        {
            List<Match> matches = new List<Match>();

            //List<Match> tmpList = scheduleRoundRobin(g.Seeds.ToList());
            //List<Match> tmpList = scheduleRoundRobinManual(g.Seeds.ToList());
            List<Match> tmpList = GenerateRoundRobinMatchesCircleMethod(g.Seeds.ToList(), g.GroupHasRematch);

            tmpList.ForEach(m => {
                m.GroupId = g.GroupId;
                m.Status = Match.MatchStatus.Created;
            });
            matches.AddRange(tmpList);

            return matches;
        }

        static List<Match> GenerateRoundRobinMatchesCircleMethod(List<Seed> seeds, bool allowRematches)
        {
            List<Match> matches = new List<Match>();

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
                        matches.Add(new Match() { Seed1Id = seed1Id, Seed2Id = seed2Id, MatchStage = stage });
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

            if (allowRematches)
            {
                int matchCount = matches.Count;
                for (int i = 0; i < matchCount; i++)
                {
                    matches.Add(new Match() { Seed1Id = matches[i].Seed2Id, Seed2Id = matches[i].Seed1Id, MatchStage = matches[i].MatchStage + totalStages });
                }
            }

            return matches;
        }

        private static List<Match> scheduleRoundRobinManual(List<Seed> seeds)
        {
            List<Match> matches = new List<Match>();
            if (seeds.Count == 3)
            {
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(1).SeedId, MatchOrderValue = 0 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(1).SeedId, Seed2Id = seeds.ElementAt(2).SeedId, MatchOrderValue = 1 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(2).SeedId, MatchOrderValue = 2 });

                matches.Add(new Match() { Seed1Id = seeds.ElementAt(2).SeedId, Seed2Id = seeds.ElementAt(1).SeedId, MatchOrderValue = 3 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(2).SeedId, Seed2Id = seeds.ElementAt(0).SeedId, MatchOrderValue = 4 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(1).SeedId, Seed2Id = seeds.ElementAt(0).SeedId, MatchOrderValue = 5 });
            }
            else if (seeds.Count == 4)
            {
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(1).SeedId, MatchOrderValue = 1 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(2).SeedId, Seed2Id = seeds.ElementAt(3).SeedId, MatchOrderValue = 1 });

                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(2).SeedId, MatchOrderValue = 3 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(1).SeedId, Seed2Id = seeds.ElementAt(3).SeedId, MatchOrderValue = 3 });

                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(3).SeedId, MatchOrderValue = 5 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(1).SeedId, Seed2Id = seeds.ElementAt(2).SeedId, MatchOrderValue = 5 });

            }
            return matches;
        }

        #endregion
    }
}
