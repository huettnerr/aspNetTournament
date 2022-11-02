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

        public bool UpdateSeedStat(Seed s, SeedStatistics stat)
        {
            if (this.Status != MatchStatus.Finished) return false;
            if (this.Score == null) return false;

            //Update Set/Leg Stat
            if (this.Player1.Equals(s.Player))
            {
                //Won
                stat.SetsWon += this.Score.P1Sets;
                stat.LegsWon += this.Score.P1Legs;

                //Lost
                stat.SetsWon += this.Score.P2Sets;
                stat.LegsWon += this.Score.P2Legs;
            } 
            else if (this.Player2.Equals(s.Player))
            {
                //Won
                stat.SetsWon += this.Score.P2Sets;
                stat.LegsWon += this.Score.P2Legs;

                //Lost
                stat.SetsWon += this.Score.P1Sets;
                stat.LegsWon += this.Score.P1Legs;
            } 
            else return false;

            //Update Winning Stat
            if(HasPlayerWon(s.Player))
            {
                stat.MatchesWon++;
            }
            else
            {
                stat.MatchesLost++;
            }
            return true;
        }

        public bool HasPlayerWon(Player p)
        {
            if (this.Status != MatchStatus.Finished) return false;

            if(this.Score == null) return false;

            if(this.Group.Round.Scoring == Round.ScoreType.LegsOnly)
            {
                //Check for Legs
                if (this.Score.P1Legs > this.Score.P2Legs)
                {
                    //Player 1 won
                    return p.Equals(this.Player1) ? true : false;
                }
                else
                {
                    //Player 2 won
                    return p.Equals(this.Player2) ? true : false;
                }
            }
            else
            {
                //Check for Sets
                if (this.Score.P1Sets > this.Score.P2Sets)
                {
                    //Player 1 won
                    return p.Equals(this.Player1) ? true : false;
                }
                else
                {
                    //Player 2 won
                    return p.Equals(this.Player2) ? true : false;
                }
            }
        }
    }
}
