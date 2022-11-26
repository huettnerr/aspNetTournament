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

        [NotMapped] public virtual ICollection<Seed> RankedSeeds { get => Seeds.OrderByDescending(s => s.Statistics.MatchesWon).ThenByDescending(s => s.Statistics.PointsDiff).ToList(); }
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
                    SeedName = "",
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
            allSeeds.ForEach(s => s.SeedNr = seedNr++);
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
}
