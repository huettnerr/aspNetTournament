using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ChemodartsWebApp.ModelHelper
{
    public static class RoundRobinLogic
    {
        public static void UpdateSeedsInRound(Round? r)
        {
            if (r is null || r.Modus != RoundModus.RoundRobin) return;

            List<Seed> allSeeds = new List<Seed>();
            r.Groups.OrderBy(g => g.GroupId).ToList().ForEach(g => allSeeds.AddRange(g.Seeds));

            int seedNr = 1;
            allSeeds.ForEach(s => { s.SeedNr = seedNr; s.SeedName = $"Seed #{seedNr}"; seedNr++; });
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
            List<Match> tmpList = scheduleRoundRobinManual(g.Seeds.ToList());

            tmpList.ForEach(m => {
                m.GroupId = g.GroupId;
                m.Status = Match.MatchStatus.Created;
            });
            matches.AddRange(tmpList);

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

        private static List<Match> scheduleRoundRobin(List<Seed> seeds)
        {
            List<Match> matches = new List<Match>();

            foreach (Tuple<int, int> pair in CreatePairs(seeds.Count))
            {
                matches.Add(new Match()
                {
                    Seed1Id = seeds.ElementAt(pair.Item1 - 1).SeedId,
                    Seed2Id = seeds.ElementAt(pair.Item2 - 1).SeedId,
                    MatchOrderValue = 0,
                });
            }

            return matches;
        }

        private static List<Tuple<int, int>> CreatePairs(int n)
        {
            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

            int[] orig = new int[n];
            for (int i = 0; i < n; i++)
            {
                orig[i] = i + 1;
            }
            IEnumerable<int> rev = orig.Reverse();

            int len = orig.Length;
            for (int j = 0; j < len - 1; j++)
            {
                List<int> tmp = new List<int>();
                tmp.Add(orig[0]);
                tmp.AddRange(rev.Take(j).Reverse());
                if (j < len && len > 1 + j) tmp.AddRange(orig.Skip(1).Take(len - 1 - j));
                pairs.AddRange(makeMatches(tmp, j + 1));
            }

            return pairs;
        }

        private static List<Tuple<int, int>> makeMatches(IEnumerable<int> arr, int round)
        {
            int halfSize = arr.Count() / 2;

            IEnumerable<int> A = arr.Take(halfSize);
            IEnumerable<int> B = arr.Skip(halfSize).Take(halfSize).Reverse();

            return A.Zip(B, (x, y) => new Tuple<int, int>(x,y)).ToList();
        }

        #endregion
    }
}
