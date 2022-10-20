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
        [Display(Name = "Seed Name")][Column("seedName")] public string? SeedName { get; set; }

        //Navigation
        [Display(Name = "Gruppe")] public virtual Group Group { get; set; }
        public virtual MapTournamentPlayer? MappedTournamentPlayer { get; set; }
        [Display(Name = "Spieler")][DisplayFormat(NullDisplayText = "n. A.")][NotMapped] public virtual Player? Player { get => MappedTournamentPlayer?.Player; }
        [Display(Name = "Turnier")][DisplayFormat(NullDisplayText = "n. A.")][NotMapped] public virtual Tournament? Tournament { get => MappedTournamentPlayer?.Tournament; }
    }
}
