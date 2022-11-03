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

}
