using ChemodartsWebApp.Data;
using ChemodartsWebApp.ModelHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

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
        [Column("matchOrderValue")] public int? MatchOrderValue { get; set; }
        [Column("matchStage")] public int? MatchStage { get; set; } // Defines the
        [Display(Name = "GroupId")][Column("groupId")] public int GroupId { get; set; }
        [Column("seed1Id")] public int? Seed1Id { get; set; }
        [Column("seed2Id")] public int? Seed2Id { get; set; }
        [Column("winnerSeedId")] public int? WinnerSeedId { get; set; }
        [Display(Name = "Status")][Column("status")] public MatchStatus Status { get; set; }
        [Column("venueId")] public int? VenueId {get; set; }

        [Display(Name = "Startzeit")][Column("time_started")][DataType(DataType.DateTime)]
        public DateTime? TimeStarted { get; set; }

        [Display(Name = "Endzeit")][Column("time_finished")][DataType(DataType.DateTime)]
        public DateTime? TimeFinished { get; set; }
        [Column("winnerFollowUpMatchId")] public int? WinnerFollowUpMatchId { get; set; }
        [Column("loserFollowUpMatchId")] public int? LoserFollowUpMatchId { get; set; }
        [Column("winnerFollowUpSeedNr")] public int? WinnerFollowUpSeedNr { get; set; }
        [Column("loserFollowUpSeedNr")] public int? LoserFollowUpSeedNr { get; set; }

        //Navigation
        public virtual Venue? Venue { get; set; }
        public virtual Group Group { get; set; }
        [Display(Name = "Heim")] 
        public virtual Seed? Seed1 { get; set; }
        [Display(Name = "Gast")] 
        public virtual Seed? Seed2 { get; set; }
        public virtual Seed? WinnerSeed { get; set; }
        [NotMapped] public virtual Seed? LoserSeed { get; set; }    
        public virtual Score? Score { get; set; }
        public virtual Match? WinnerFollowUpMatch { get; set; }
        public virtual Match? LoserFollowUpMatch { get; set; }
        public virtual ICollection<Match>? AncestorMatchesAsWinner { get; set; } = new List<Match>(); 
        public virtual ICollection<Match>? AncestorMatchesAsLoser { get; set; } = new List<Match>();
        [NotMapped] public ICollection<Match>? AncestorMatches { get { return AncestorMatchesAsWinner?.Concat(AncestorMatchesAsLoser)?.ToList(); } }

        //Helpers
        [NotMapped]
        public List<SelectListItem> AvailableVenues
        {
            get
            {
                List<SelectListItem> res = new List<SelectListItem>();
                if (Venue is object) res.Add(new SelectListItem($"{Venue.VenueName} (aktuell)", Venue.VenueId.ToString(), true));
                Group?.Round?.MappedVenues?.Select(mv => mv.Venue).Where(v => v?.Match is null).ToList().
                    ForEach(v => res.Add(new SelectListItem(v.VenueName, v.VenueId.ToString())));
                return res;
            }
        }

        public bool IsWinnerSeed(Seed? s)
        {
            if (s is null) return false;

            return s.Equals(WinnerSeed);
        }

        public bool IsMatchOfSeeds(Seed? s1, Seed? s2)
        {
            if (Seed1 is null || Seed2 is null || s1 is null || s2 is null) return false;

            if (Seed1.Equals(s1) && Seed2.Equals(s2) || Seed1.Equals(s2) && Seed2.Equals(s1))
            {
                return true;
            }

            return false;
        }

        public bool IsMatchOfPlayers(Player p1, Player p2)
        {
            if (Seed1?.Player is null || Seed2?.Player is null) return false;

            if (Seed1.Player.Equals(p1) && Seed2.Player.Equals(p2) || Seed1.Player.Equals(p2) && Seed2.Player.Equals(p1))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handles the match and sets winner and loser seed
        /// </summary>
        /// <param name="m">The match to be handled</param>
        /// <returns>True if the match has a winner</returns>
        public bool HandleWinnerLoserSeedOfMatch()
        {
            //No seeds set yet or both are dummys
            if ((Seed1?.IsDummy ?? true) && (Seed2?.IsDummy ?? true)) return false;

            if ((Seed1 is object && !Seed1.IsDummy) && (Seed2 is object && Seed2.IsByeSeed()))
            {
                //Seed 1 has bye
                WinnerSeed = Seed1;
                LoserSeed = Seed2;
                Status = MatchStatus.Finished;
                return true;
            }
            else if ((Seed2 is object && !Seed2.IsDummy) && (Seed1 is object && Seed1.IsByeSeed()))
            {
                //Seed 2 has bye
                WinnerSeed = Seed2;
                LoserSeed = Seed1;
                Status = MatchStatus.Finished;
                return true;
            }

            if (Status != MatchStatus.Finished) goto CLEAR;
            if (Score == null) goto CLEAR;

            if (Group.Round.Scoring == ScoreType.LegsOnly)
            {
                //Check for Legs
                if (Score.P1Legs > Score.P2Legs)
                {
                    //Seed 1 won
                    WinnerSeed = Seed1;
                    LoserSeed = Seed2;
                    return true;
                }
                else if (Score.P1Legs < Score.P2Legs)
                {
                    //Seed 1 won
                    WinnerSeed = Seed2;
                    LoserSeed = Seed1;
                    return true;
                }
            }
            else
            {
                //Check for Sets
                if (Score.P1Sets > Score.P2Sets)
                {
                    //Seed 1 won
                    WinnerSeed = Seed1;
                    LoserSeed = Seed2;
                    return true;
                }
                else if (Score.P1Sets < Score.P2Sets)
                {
                    //Seed 1 won
                    WinnerSeed = Seed2;
                    LoserSeed = Seed1;
                    return true;
                }
            }

        CLEAR:
            WinnerSeed = null;
            LoserSeed = null;
            return false;
        }

        public override string ToString()
        {
            return $"[{MatchStage}|{MatchOrderValue}] {Seed1?.ToString()} vs. {Seed2?.ToString()}";
        }
    }
}
