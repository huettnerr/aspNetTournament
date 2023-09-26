using ChemodartsWebApp.Data.Factory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("players")]
    public class Player
    {
        [Key][Display(Name = "ID")][Column("playerId")] public int PlayerId { get; set; }
        [Display(Name = "Name")][Column("name")] public string? PlayerName { get; set; }
        [Display(Name = "Dartname")][Column("dartname")] public string? PlayerDartname { get; set; }
        [NotMapped][Display(Name = "Spieler")][DisplayFormat(NullDisplayText = "n. A.")] public string CombinedName { get => $"{PlayerName} \"{PlayerDartname}\""; }
        [Display(Name = "Kontakt")][Column("contactData")] public string? PlayerContactData { get; set; }
        [Display(Name = "Interpret")][Column("interpret")] public string? PlayerInterpret { get; set; }
        [Display(Name = "Einlaufsong")][Column("song")] public string? PlayerSong { get; set; }

        //Navigation
        public virtual ICollection<MapRoundSeedPlayer>? MappedTournaments { get; set; }
        [NotMapped] public virtual ICollection<Seed>? Seeds { get => MappedTournaments?.Select(mtp => mtp.Seed).ToList(); }
        [NotMapped] public virtual ICollection<Match>? Matches { get => Seeds?.SelectMany(s => s.Matches).Distinct().ToList(); } 
    }
}
