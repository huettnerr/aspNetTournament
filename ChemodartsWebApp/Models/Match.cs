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
        [Column("matchOrderValue")] public int MatchOrderValue { get; set; }
        [Display(Name = "GroupId")][Column("groupId")] public int GroupId { get; set; }
        [Column("seed1Id")] public int Seed1Id { get; set; }
        [Column("seed2Id")] public int Seed2Id { get; set; }
        [Display(Name = "Status")][Column("status")] public MatchStatus? Status {get; set; }
        [Column("venueId")] public int? VenueId {get; set; }

        [Display(Name = "Startzeit")][Column("time_started")][DataType(DataType.DateTime)]
        public DateTime? TimeStarted { get; set; }

        [Display(Name = "Endzeit")][Column("time_finished")][DataType(DataType.DateTime)]
        public DateTime? TimeFinished { get; set; }

        //Navigation
        public virtual Venue? Venue { get; set; }
        public virtual Group Group { get; set; }
        [Display(Name = "Heim")] 
        public virtual Seed Seed1 { get; set; }
        [Display(Name = "Gast")] 
        public virtual Seed Seed2 { get; set; }
        public virtual Score? Score { get; set; }


        //[NotMapped][Display(Name = "Heim")] public virtual Player? Player1 { get => Seed1.Player; }
        //[NotMapped][Display(Name = "Gast")] public virtual Player? Player2 { get => Seed2.Player; }

        public bool UpdateSeedStat(Seed s, SeedStatistics stat)
        {
            if (this.Status != MatchStatus.Finished) return false;
            if (this.Score == null) return false;

            //Update Set/Leg Stat
            if (this.Seed1.Equals(s))
            {
                //Won
                stat.SetsWon += this.Score.P1Sets;
                stat.LegsWon += this.Score.P1Legs;

                //Lost
                stat.SetsWon += this.Score.P2Sets;
                stat.LegsWon += this.Score.P2Legs;
            } 
            else if (this.Seed2.Equals(s))
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
            if(HasSeedWon(s))
            {
                stat.MatchesWon++;
            }
            else
            {
                stat.MatchesLost++;
            }
            return true;
        }

        public bool HasSeedWon(Seed s)
        {
            if (this.Status != MatchStatus.Finished) return false;

            if(this.Score == null) return false;

            if(this.Group.Round.Scoring == Round.ScoreType.LegsOnly)
            {
                //Check for Legs
                if (this.Score.P1Legs > this.Score.P2Legs)
                {
                    //Seed 1 won
                    return s.Equals(this.Seed1) ? true : false;
                }
                else
                {
                    //Seed 2 won
                    return s.Equals(this.Seed2) ? true : false;
                }
            }
            else
            {
                //Check for Sets
                if (this.Score.P1Sets > this.Score.P2Sets)
                {
                    //Player 1 won
                    return s.Equals(this.Seed1) ? true : false;
                }
                else
                {
                    //Player 2 won
                    return s.Equals(this.Seed2) ? true : false;
                }
            }
        }
    }
}
