using ChemodartsWebApp.Data;
using ChemodartsWebApp.ModelHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "Status")][Column("status")] public MatchStatus Status { get; private set; } = Match.MatchStatus.Created;
        [Column("venueId")] public int? VenueId {get; set; }

        [Display(Name = "Startzeit")][Column("time_started")][DataType(DataType.DateTime)]
        public DateTime? TimeStarted { get; set; }

        [Display(Name = "Endzeit")][Column("time_finished")][DataType(DataType.DateTime)]
        public DateTime? TimeFinished { get; set; }
        [Column("followUpMatchId")] public int? FollowUpMatchId { get; set; }

        //Navigation
        public virtual Venue? Venue { get; set; }
        public virtual Group Group { get; set; }
        [Display(Name = "Heim")] 
        public virtual Seed? Seed1 { get; set; }
        [Display(Name = "Gast")] 
        public virtual Seed? Seed2 { get; set; }
        public virtual Seed? WinnerSeed { get; set; }
        public virtual Score? Score { get; set; }
        public virtual Seed? WinnerSeedFollowUp { get; set; }
        public virtual Match? FollowUpMatch { get; set; }
        public virtual ICollection<Match>? AncestorMatches { get; set; }

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

        //Functions

        public void SetNewStatus(MatchStatus newStatus)
        {
            Status = newStatus;
            switch(newStatus)
            {
                case MatchStatus.Created:
                    Score = null;
                    break;
                case MatchStatus.Finished: 
                    Venue = null;
                    break;
            }

            WinnerSeed = Ranking.GetWinnerSeed(this);

            switch(Group.Round.Modus)
            {
                case RoundModus.RoundRobin:
                    Ranking.UpdateGroupRanking(Group);
                    break;
                case RoundModus.SingleKo:
                case RoundModus.DoubleKo:
                    UpdateTopTierFollowUp();
                    break;
            }
        }

        public void UpdateTopTierFollowUp()
        {
            if(FollowUpMatch?.FollowUpMatch is object)
            {
                //Follow Up is not the top tier
                FollowUpMatch.UpdateTopTierFollowUp();
            }
            else
            {
                if(FollowUpMatch is object)
                {
                    //FollowUp os the top tier
                    FollowUpMatch.UpdateSeedsFromAcestors();
                }
                else
                {
                    //we are the top tier
                    this.UpdateSeedsFromAcestors();
                }
            }
        }

        //Recursivly updates from their ancestor matches
        public void UpdateSeedsFromAcestors()
        {
            if (AncestorMatches?.Count == 2)
            {
                Match am1 = AncestorMatches.OrderBy(m => m.MatchOrderValue).ElementAt(0);
                this.updateSeedFromAncestor(this, am1, this.Seed1, out Seed s1New);
                this.Seed1 = s1New;

                Match am2 = AncestorMatches.OrderBy(m => m.MatchOrderValue).ElementAt(1);
                this.updateSeedFromAncestor(this, am2, this.Seed2, out Seed s2New);
                this.Seed2 = s2New;
            }
        }

        private void updateSeedFromAncestor(Match m, Match ancestorMatch, Seed? S1orS2, out Seed newSeed)
        {
            ancestorMatch.UpdateSeedsFromAcestors();
            if (ancestorMatch.WinnerSeed is object)
            {
                //Match has winner
                newSeed = ancestorMatch.WinnerSeed;
            }
            else
            {
                //Match has no winner
                newSeed = S1orS2 is object ? S1orS2 : new Seed() { GroupId = m.GroupId };

                //Update Seedname
                List<string?> sb = new List<string?>
                {
                    ancestorMatch.Seed1?.Player?.PlayerDartname ?? ancestorMatch.Seed1?.SeedName,
                    ancestorMatch.Seed2?.Player?.PlayerDartname ?? ancestorMatch.Seed2?.SeedName
                };
                newSeed.SeedName = String.Join(" | ", sb.Where(s => s is object));
            }
        }

        public bool IsWinnerSeed(Seed? s)
        {
            if (s is null) return false;

            return s.Equals(WinnerSeed);
        }

        public bool IsMatchOfSeeds(Seed? s1, Seed? s2)
        {
            if(Seed1 is null || Seed2 is null || s1 is null || s2 is null) return false;

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

        public override string ToString()
        {
            return $"[{MatchStage}|{MatchOrderValue}] {Seed1?.ToString()} vs. {Seed2?.ToString()}";
        }
    }
}
