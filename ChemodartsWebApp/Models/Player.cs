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
        [NotMapped][Display(Name = "Spieler")][DisplayFormat(NullDisplayText = "n. A.")] public string? CombinedName { get => $"{PlayerName} \"{PlayerDartname}\""; }
        [Display(Name = "Kontakt")][Column("contactData")] public string? PlayerContactData { get; set; }
        [Display(Name = "Interpret")][Column("interpret")] public string? PlayerInterpret { get; set; }
        [Display(Name = "Einlaufsong")][Column("song")] public string? PlayerSong { get; set; }

        //Navigation
        public virtual ICollection<MapTournamentPlayer> MappedTournaments { get; set; }
        public virtual ICollection<Match> MatchesHome { get; set; }
        public virtual ICollection<Match> MatchesAway { get; set; }
        [NotMapped] public ICollection<Match> Matches { get { return MatchesHome.Concat(MatchesAway).OrderByDescending(m => m.Status).ThenBy(m => m.TimeStarted).ToList(); } }    
    }
}
