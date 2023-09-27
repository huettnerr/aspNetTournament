using ChemodartsWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ChemodartsWebApp.Data.Factory
{
    public class GroupFactory : FactoryBase<Group>
    {
        public override string Controller { get; } = "Group";
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public Round? R { get; set; }

        public GroupFactory() { } //Needed for POST
        public GroupFactory(string action, Group? g) : base(action)
        {
            if (g is object)
            {
                Name = g.GroupName;
            }
            else
            {
                Name = String.Empty;
            }
        }

        public override Group? Create()
        {
            if (R is null) return null;

            Group g = new Group();
            Update(ref g);
            g.RoundId = R.RoundId;

            return g;
        }

        public override void Update(ref Group g)
        {
            g.GroupName = Name;
        }
    }

    public class GroupFactoryRR : GroupFactory
    {
        public int PlayersPerGroup { get; set; }

        public GroupFactoryRR() { } //Needed for POST
        public GroupFactoryRR(string action) : base(action, null)
        {
            PlayersPerGroup = 0;
        }

        public List<Seed>? CreateSeeds(Group? g)
        {
            if (g is null) return null;

            List<Seed> seeds = new List<Seed>();
            for (int i = 0; i < PlayersPerGroup; i++)
            {
                seeds.Add(new Seed()
                {
                    SeedNr = 0,
                    SeedName = "Seed #0",
                    GroupId = g.GroupId,
                });
            }

            return seeds;
        }

        public static void UpdateSeeds(ICollection<Group> groups)
        {
            List<Seed> allSeeds = new List<Seed>();
            groups.ToList().ForEach(g => allSeeds.AddRange(g.Seeds));

            int seedNr = 1;
            allSeeds.ForEach(s => { s.SeedNr = seedNr; s.SeedName = $"Seed #{seedNr}"; seedNr++; });
        }

        public List<MapRoundSeedPlayer>? CreateMapping(Round? r, List<Seed>? seeds)
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
                }); ;
            }

            return mtsps;
        }
    }

    public class GroupFactoryKO : GroupFactory
    {
        public int NumberOfRounds { get; set; }

        public GroupFactoryKO() { } //Needed for POST
        public GroupFactoryKO(string action) : base(action, null)
        {
            NumberOfRounds = 0;
        }

        public override Group? Create()
        {
            return null;
        }

        public bool CreateSystem(ChemodartsContext context)
        {
            if (R is null) return false;

            NumberOfRounds--; // Fühlt sich natürlicher an das Finale mitzuzählen

            List<Group> groups = new List<Group>();
            for (int roundNr = 0; roundNr <= NumberOfRounds; roundNr++)
            {

                int playersInRound = 2 * getMatchesPerRound(NumberOfRounds - roundNr);
                Group g = new Group()
                {
                    GroupName = getGroupName(playersInRound),
                    RoundId = R.RoundId,
                };
                g.Matches = new List<Match>();
                groups.Add(g);
                context.Groups.Add(g);
                context.SaveChanges();

                //Make Seeds
                List<Seed> seeds = new List<Seed>();
                for (int iPlayer = 0; iPlayer < playersInRound; iPlayer++)
                {
                    Seed s = new Seed()
                    {
                        SeedName = "Please Run Script",
                        GroupId = g.GroupId,
                    };

                    if (roundNr > 1)
                    {
                        //Link Ancestors
                        s.AncestorMatch = groups.ElementAt(roundNr - 1).Matches.ElementAt(iPlayer);
                    }
                    else
                    {
                        //First round
                        s.SeedNr = iPlayer;
                    }

                    seeds.Add(s);
                }
                context.Seeds.AddRange(seeds);
                context.SaveChanges();

                //Make Matches
                List<Match> matches = new List<Match>();
                for (var iMatch = 0; iMatch < getMatchesPerRound(NumberOfRounds - roundNr); iMatch++)
                {
                    Match m = new Match()
                    {
                        Seed1Id = seeds.ElementAt(2 * iMatch).SeedId,
                        Seed2Id = seeds.ElementAt(2 * iMatch + 1).SeedId,
                        MatchOrderValue = iMatch,
                        GroupId = g.GroupId,
                        Status = Match.MatchStatus.Created,
                    };
                    g.Matches.Add(m);
                    matches.Add(m);
                }
                context.Matches.AddRange(matches);
                context.SaveChanges();
            }

            return true;
        }

        public static void UpdateFirstRoundSeeds(Data.ChemodartsContext context, Round previousRound, Group firstKoRoundGroup)
        {
            int numberOfMatches = firstKoRoundGroup.Matches.Count;
            int matchNr = 0;
            foreach (Match m in firstKoRoundGroup.Matches)
            {
                if ((2 * numberOfMatches) == previousRound.Groups.Count)
                {
                    Seed sforS1 = previousRound.Groups.ElementAt(2 * matchNr).RankedSeeds.ElementAt(0);
                    m.Seed1Id = sforS1.SeedId;
                    m.Seed1.SeedName = sforS1.Player?.PlayerDartname ?? sforS1.SeedName;

                    Seed sforS2 = previousRound.Groups.ElementAt(2 * matchNr + 1).RankedSeeds.ElementAt(0);
                    m.Seed2Id = sforS2.SeedId;
                    m.Seed2.SeedName = sforS2.Player?.PlayerDartname ?? sforS2.SeedName;
                }
                else if (numberOfMatches == previousRound.Groups.Count)
                {
                    Seed sforS1 = previousRound.Groups.ElementAt(matchNr).RankedSeeds.ElementAt(0);
                    m.Seed1Id = sforS1.SeedId;
                    m.Seed1.SeedName = sforS1.Player?.PlayerDartname ?? sforS1.SeedName;

                    Seed sforS2 = previousRound.Groups.ElementAt(numberOfMatches - 1 - matchNr).RankedSeeds.ElementAt(1);
                    m.Seed2Id = sforS2.SeedId;
                    m.Seed2.SeedName = sforS2.Player?.PlayerDartname ?? sforS2.SeedName;
                }
                else
                {
                    //Group count does not match
                    return;
                }
                matchNr++;
            }

            context.SaveChanges();
        }

        public static void UpdateKoRoundSeeds(Data.ChemodartsContext context, Round r)
        {
            for (int i = 1; i < r.Groups.Count; i++)
            {
                int matchNr = 0;
                foreach (Match m in r.Groups.ElementAt(i).Matches)
                {
                    Match mForS1 = r.Groups.ElementAt(i - 1).Matches.ElementAt(2 * matchNr);
                    if (mForS1.WinnerSeed is object) m.Seed1Id = mForS1.WinnerSeed.SeedId;
                    else m.Seed1.SeedName = $"{mForS1.Seed1.Player?.PlayerDartname ?? mForS1.Seed1.SeedName} | {mForS1.Seed2.Player?.PlayerDartname ?? mForS1.Seed2.SeedName}";

                    Match mForS2 = r.Groups.ElementAt(i - 1).Matches.ElementAt(2 * matchNr + 1);
                    if (mForS2.WinnerSeed is object) m.Seed2Id = mForS2.WinnerSeed.SeedId;
                    else m.Seed2.SeedName = $"{mForS2.Seed1.Player?.PlayerDartname ?? mForS2.Seed1.SeedName} | {mForS2.Seed2.Player?.PlayerDartname ?? mForS2.Seed2.SeedName}";

                    matchNr++;
                }
            }

            context.SaveChanges();
        }

        private int getMatchesPerRound(int depth)
        {
            return Convert.ToInt32(Math.Pow(2, depth));
        }

        private string getGroupName(int players)
        {
            switch (players)
            {
                case 2: return "Finale";
                case 4: return "Halbfinale";
                case 8: return "Viertelfinale";
                //case 16: return "Finale";
                default:
                    return $"Stufe der besten {players}";
            }
        }
    }
}
