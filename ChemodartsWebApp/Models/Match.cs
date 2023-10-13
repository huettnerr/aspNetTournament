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
        [Display(Name = "Status")][Column("status")] public MatchStatus Status { get; private set; } = Match.MatchStatus.Created;
        [Column("venueId")] public int? VenueId {get; set; }

        [Display(Name = "Startzeit")][Column("time_started")][DataType(DataType.DateTime)]
        public DateTime? TimeStarted { get; set; }

        [Display(Name = "Endzeit")][Column("time_finished")][DataType(DataType.DateTime)]
        public DateTime? TimeFinished { get; set; }
        [Column("winnerFollowUpMatchId")] public int? WinnerFollowUpMatchId { get; set; }
        [Column("loserFollowUpMatchId")] public int? LoserFollowUpMatchId { get; set; }

        //Navigation
        public virtual Venue? Venue { get; set; }
        public virtual Group Group { get; set; }
        [Display(Name = "Heim")] 
        public virtual Seed? Seed1 { get; set; }
        [Display(Name = "Gast")] 
        public virtual Seed? Seed2 { get; set; }
        public virtual Seed? WinnerSeed { 
            get; 
            private set; 
        }
        [NotMapped] public virtual Seed? LoserSeed { get; private set; }    
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

            UpdateUpwards();
        }

        public void UpdateUpwards()
        {
            // update
            if (WinnerFollowUpMatch is null && LoserFollowUpMatch is null)
            {
                // update when no follow ups
                Update();
            }
            else
            {
                // propagate upwards
                WinnerFollowUpMatch?.UpdateUpwards();
                LoserFollowUpMatch?.UpdateUpwards();
            }
        }

        public void Update()
        {
            switch (Group.Round.Modus)
            {
                case RoundModus.RoundRobin:
                    UpdateWinnerSeed(this);
                    Ranking.UpdateGroupRanking(Group);
                    break;
                case RoundModus.SingleKo:
                    //goto case RoundModus.DoubleKo;
                case RoundModus.DoubleKo:
                    UpdateSeedsFromAcestors(this);
                    //PropagateToTopTier(nameof(LoserFollowUpMatch));
                    break;
            }
        }

        //Recursivly updates from their ancestor matches
        public static void UpdateSeedsFromAcestors(Match m)
        {
            if (m.AncestorMatches?.Count == 2)
            {
                Match am1 = m.AncestorMatches.OrderBy(m => m.MatchOrderValue).ElementAt(0);
                bool isWinnerAncestor = m.AncestorMatchesAsWinner?.Contains(am1) ?? false;
                Match.updateSeedFromAncestor(m.GroupId, am1, m.Seed1, out Seed? s1New, isWinnerAncestor);
                m.Seed1 = s1New;

                Match am2 = m.AncestorMatches.OrderBy(m => m.MatchOrderValue).ElementAt(1);
                isWinnerAncestor = m.AncestorMatchesAsWinner?.Contains(am2) ?? false;
                Match.updateSeedFromAncestor(m.GroupId, am2, m.Seed2, out Seed? s2New, isWinnerAncestor);
                m.Seed2 = s2New;
            }
            else
            {
                //Seed has no ancesters, so add their name/player name to the list
                if(m.Seed1 is object) m.Seed1.PossibleSeeds = new List<string?>() { m.Seed1?.Player?.ShortName ?? m.Seed1?.SeedName };
                if(m.Seed2 is object) m.Seed2.PossibleSeeds = new List<string?>() { m.Seed2?.Player?.ShortName ?? m.Seed2?.SeedName };
            }
        }

        private static void updateSeedFromAncestor(int baseGroupId, Match ancestorMatch, Seed? S1orS2, out Seed? newSeed, bool useWinnerSeed)
        {
            UpdateSeedsFromAcestors(ancestorMatch);

            UpdateWinnerSeed(ancestorMatch);
            if (UpdateWinnerSeed(ancestorMatch))
            {
                //Match has winner
                newSeed = useWinnerSeed ? ancestorMatch.WinnerSeed : ancestorMatch.LoserSeed;
                newSeed?.PossibleSeeds.Clear();
                newSeed?.PossibleSeeds.Add(newSeed?.Player?.ShortName ?? newSeed?.SeedName);
            }
            else //Match has no winner
            {
                //check if this match already has a dummy seed and create if not
                newSeed = S1orS2 is object ? S1orS2 : new Seed() { GroupId = baseGroupId };

                //Update possible seeds
                newSeed.PossibleSeeds.Clear();
                newSeed.PossibleSeeds.AddRange(ancestorMatch.Seed1?.PossibleSeeds ?? new List<string?>());
                newSeed.PossibleSeeds.AddRange(ancestorMatch.Seed2?.PossibleSeeds ?? new List<string?>());

                //update the name
                if(newSeed.PossibleSeeds.Count <= 12)
                {
                    newSeed.SeedName = String.Join(" | ", newSeed.PossibleSeeds.Where(s => s is object));
                }
                else
                {
                    newSeed.SeedName = $"{newSeed.PossibleSeeds.Count} possible seeds";
                }

            }
        }


        //public void PropagateToTopTier(string propertyName)
        //{
        //    //Make sure only winner or loser follow up match property is passed
        //    if (!(propertyName.Equals(nameof(Match.WinnerFollowUpMatch)) || propertyName.Equals(nameof(Match.LoserFollowUpMatch)))) return;
        //    PropertyInfo followUpProperty = typeof(Match).GetProperty(propertyName);

        //    //get the (winner or loser) follow up match property
        //    Match? followUpMatch = followUpProperty?.GetValue(this) as Match;
        //    Match? followUpMatchsFollowUpMatch = followUpMatch is object ? followUpProperty?.GetValue(followUpMatch) as Match : null;

        //    //propagate correctly
        //    if(followUpMatchsFollowUpMatch is object)
        //    {
        //        //Follow Up is not the top tier
        //        followUpMatch.PropagateToTopTier(propertyName);
        //    }
        //    else
        //    {
        //        //when follow up match is an object, than its the top tier, otherwise we are
        //        UpdateSeedsFromAcestors(followUpMatch ?? this);
        //    }
        //}

        public bool IsWinnerSeed(Seed? s)
        {
            if (s is null) return false;

            return s.Equals(WinnerSeed);
        }

        private static bool UpdateWinnerSeed(Match m)
        {
            m.WinnerSeed = Ranking.GetWinnerSeed(m);

            if (m.WinnerSeed is null)
            {
                m.LoserSeed = null;
                return false;
            }
            else
            {
                m.LoserSeed = m.IsWinnerSeed(m.Seed1) ? m.Seed2 : m.Seed1;
                return true;
            }
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
