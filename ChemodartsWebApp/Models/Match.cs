using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("matches")]
    public class Match
    {
        public enum MatchStatus
        {
            [Display(Name = "Erstellt")] Created,
            [Display(Name = "Läuft")] Active,
            [Display(Name = "Beendet")] Finished
        }

        [Key][Display(Name = "ID")][Column("matchId")] public int MatchId { get; set; }
        [Display(Name = "Heim")][Column("player1Id")] public int Player1Id { get; set; }
        [Display(Name = "Gast")][Column("player2Id")] public int Player2Id { get; set; }
        [Display(Name = "Status")][Column("status")] public MatchStatus Status {get; set; }

        [Display(Name = "Startzeit")][Column("time_started")][DataType(DataType.DateTime)]
        public DateTime? TimeStarted { get; set; }

        [Display(Name = "Endzeit")][Column("time_finished")][DataType(DataType.DateTime)]
        public DateTime? TimeFinished { get; set; }
    }
}
