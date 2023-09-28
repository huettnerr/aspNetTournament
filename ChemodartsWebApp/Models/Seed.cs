using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("seeds")]
    public class Seed
    {
        [Key][Column("seedId")] public int SeedId { get; set; }
        [Column("groupId")] public int GroupId { get; set; }
        [Display(Name = "Seed")][Column("seedNr")] public int SeedNr { get; set; }
        [Display(Name = "Rank")][Column("seedRank")] public int SeedRank { get; set; }
        [Display(Name = "Seed Name")][Column("seedName")] public string? SeedName { get; set; }
        [Column("ancestorMatchId")] public int? AncestorMatchId { get; set; }

        //Navigation
        [Display(Name = "Gruppe")] public virtual Group Group { get; set; }
        [Display(Name = "Stats")][DisplayFormat(NullDisplayText = "n. A.")] public virtual SeedStatistics? SeedStatistics { get; set; }
        public virtual MapRoundSeedPlayer MappedRoundSeedPlayer { get; set; }
        public virtual Match? AncestorMatch { get; set; }
        public virtual ICollection<Match> MatchesAsS1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsS2 { get; set; } = new List<Match>();

        [NotMapped] public ICollection<Match> Matches { get { return MatchesAsS1.Concat(MatchesAsS2).ToList(); } }
        //[NotMapped][Display(Name = "Turnier")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Tournament Tournament { get => MappedTournamentPlayer.Round.Tournament; }
        [NotMapped][Display(Name = "Runde")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Round Round { get => MappedRoundSeedPlayer?.Round; }
        [NotMapped][Display(Name = "Spieler")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Player? Player { get => MappedRoundSeedPlayer?.Player; }
    }
}
