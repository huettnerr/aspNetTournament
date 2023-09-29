using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("groups")]
    public class Group
    {
        [Key][Display(Name = "ID")][Column("groupId")] public int GroupId { get; set; }
        [Display(Name = "RoundID")][Column("roundId")] public int RoundId { get; set; }
        [Required][Display(Name = "Gruppe")][Column("name")] public string GroupName { get; set; }
        [Display(Name = "Rückspiel")][Column("hasRematch")] public bool GroupHasRematch { get; set; }

        //Navigation
        public virtual Round Round { get; set; }
        //public virtual ICollection<MapGroupPlayer> MappedPlayers { get; set; }
        [NotMapped] public virtual ICollection<Match> Matches { get; set; }
        [NotMapped] public virtual ICollection<Seed> Seeds { get ; set; }

        [NotMapped] public virtual ICollection<Seed> RankedSeeds {  get => Seeds.OrderBy(s => s.SeedRank).ToList(); }
        [NotMapped] public virtual ICollection<Match> OrderedMatches { get => Matches.OrderBy(m => m.MatchOrderValue).ToList(); }

        public override string ToString()
        {
            return $"[{GroupId}] \"{GroupName}\"";
        }
    }
}
