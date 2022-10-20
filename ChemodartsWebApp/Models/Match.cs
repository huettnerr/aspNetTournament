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
            [Display(Name = "Beendet")] Finished,
            [Display(Name = "Abgebrochen")] Aborted
        }

        [Key][Display(Name = "ID")][Column("matchId")] public int MatchId { get; set; }
        [Column("player1Id")] public int Player1Id { get; set; }
        [Column("player2Id")] public int Player2Id { get; set; }
        [Display(Name = "Status")][Column("status")] public MatchStatus? Status {get; set; }

        [Display(Name = "Startzeit")][Column("time_started")][DataType(DataType.DateTime)]
        public DateTime? TimeStarted { get; set; }

        [Display(Name = "Endzeit")][Column("time_finished")][DataType(DataType.DateTime)]
        public DateTime? TimeFinished { get; set; }

        //Navigation
        [Display(Name = "GroupId")][Column("groupId")] public int GroupId { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual Group Group { get; set; }
        [Display(Name = "Heim")] public virtual Player Player1 { get; set; }
        [Display(Name = "Gast")] public virtual Player Player2 { get; set; }
        public virtual Score Score { get; set; }
    }
}
