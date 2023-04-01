using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("groups")]
    public class Group
    {
        [Key][Display(Name = "ID")][Column("groupId")] public int GroupId { get; set; }
        [Required][Display(Name = "Gruppe")][Column("name")] public string? GroupName { get; set; }

        //Navigation
        [Display(Name = "RoundID")][Column("roundId")] public int RoundId { get; set; }
        public virtual Round Round { get; set; }
        //public virtual ICollection<MapGroupPlayer> MappedPlayers { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
        public virtual ICollection<Seed> Seeds { get ; set; }

        [NotMapped] public virtual ICollection<Seed> RankedSeeds {
            get
            {
                List<Seed> rankedSeeds = new List<Seed>();
                Seeds.ToList().ForEach(s => {
                    s.UpdateSeedStatistics(RoundId);
                    rankedSeeds.Add(s);
                });
                rankedSeeds = rankedSeeds
                    .OrderByDescending(s => s.Statistics.MatchesWon)
                    .ThenByDescending(s => s.Statistics.PointsDiff)
                    .ThenByDescending(s => s.Statistics.Points).ToList();

                for (int i = 0; i < rankedSeeds.Count - 1; i++)
                {
                    //Check if some seeds are totally identical
                    if (rankedSeeds[i].Statistics.MatchesWon == rankedSeeds[i + 1].Statistics.MatchesWon &&
                        rankedSeeds[i].Statistics.PointsDiff == rankedSeeds[i + 1].Statistics.PointsDiff &&
                        rankedSeeds[i].Statistics.Points == rankedSeeds[i + 1].Statistics.Points)
                    {
                        Match? m = Matches.Where(m => m.IsMatchOfSeeds(rankedSeeds[i], rankedSeeds[i + 1])).FirstOrDefault();
                        if (m is object)
                        {
                            if (m.HasSeedWon(rankedSeeds[i + 1]))
                            {
                                Seed tmp = rankedSeeds[i];
                                rankedSeeds[i] = rankedSeeds[i + 1];
                                rankedSeeds[i + 1] = tmp;
                            }
                        }
                    }
                }
                return rankedSeeds;
            }
        }
        [NotMapped] public virtual ICollection<Match> OrderedMatches { get => Match.OrderMatches(this.Matches).ToList(); }
    }

    public class GroupFactory
    {
        public string Name { get; set; }
        public int PlayersPerGroup { get; set; }
        public int? RoundId { get; set; }

        public Group? CreateGroup()
        {
            if (RoundId is null) return null;

            Group g = new Group()
            {
                GroupName = Name,
                RoundId = RoundId ?? 0,
            };

            return g;
        }

        public List<Seed>? CreateSeeds(int groupId)
        {
            //if (tournament is null) return null;

            List<Seed> seeds = new List<Seed>();
            for (int i = 0; i < PlayersPerGroup; i++)
            {
                seeds.Add(new Seed()
                {
                    SeedNr = 0,
                    SeedName = "Seed #0",
                    GroupId = groupId,
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

        public List<MapTournamentSeedPlayer>? CreateMapping(int? tournamentId, List<Seed>? seeds)
        {
            if (tournamentId is null || seeds is null) return null;

            List<MapTournamentSeedPlayer> mtsps = new List<MapTournamentSeedPlayer>();
            foreach (Seed seed in seeds)
            {
                mtsps.Add(new MapTournamentSeedPlayer()
                {
                    TSP_SeedId = seed.SeedId,
                    TSP_TournamentId = tournamentId ?? 0,
                    TSP_PlayerCheckedIn = false,
                    TSP_PlayerFixed = false,
                    Player = null
                }); ;
            }

            return mtsps;
        }
    }

    public class KoFactory
    {
        public int NumberOfRounds { get; set; }
        public int? ThisRoundId { get; set; }

        public bool CreateSystem(Data.ChemodartsContext context)
        {
            if (ThisRoundId is null) return false;
            NumberOfRounds--; // Fühlt sich natürlicher an das Finale mitzuzählen

            List<Group> groups = new List<Group>();
            for(int roundNr = 0; roundNr <= NumberOfRounds; roundNr++)
            {

                int playersInRound = 2 * getMatchesPerRound(NumberOfRounds - roundNr);
                Group g = new Group()
                {
                    GroupName = getGroupName(playersInRound),
                    RoundId = ThisRoundId ?? 0,
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
                    }else
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
            for(int i = 1; i < r.Groups.Count; i++)
            {
                int matchNr = 0;
                foreach (Match m in r.Groups.ElementAt(i).Matches)
                {
                    Match mForS1 = r.Groups.ElementAt(i - 1).Matches.ElementAt(2 * matchNr);
                    if (mForS1.WinnerSeed is object) m.Seed1Id = mForS1.WinnerSeed.SeedId;
                    else m.Seed1.SeedName = $"{mForS1.Seed1.Player?.PlayerDartname ?? mForS1.Seed1.SeedName } | {mForS1.Seed2.Player?.PlayerDartname ?? mForS1.Seed2.SeedName}";

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
            switch(players) 
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
