using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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
        [Column("matchOrderValue")] public int? MatchOrderValue { get; set; }
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
        public virtual Seed? WinnerSeedFollowUp { get; set; }
        [NotMapped] public Seed? WinnerSeed { 
            get {
                if (HasSeedWon(Seed1)) return Seed1;
                else if (HasSeedWon(Seed2)) return Seed2;
                else return null;
            } 
        }

        //Helpers
        [NotMapped]
        [BindProperty]
        public Venue? SelectedVenue { get; set; }

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

        //[NotMapped]
        //public SelectList AvailableVenues
        //{
        //    get => new SelectList(Group?.Round?.MappedVenues?.Select(mv => mv.Venue).Where(v => v?.Match is null), "VenueId", "VenueName", Venue is object ? "No Venue" : "Select Venue");
        //}

        [NotMapped]
        [BindProperty]
        public MatchStatus? NewStatus { get; set; }


        //[NotMapped][Display(Name = "Heim")] public virtual Player? Player1 { get => Seed1.Player; }
        //[NotMapped][Display(Name = "Gast")] public virtual Player? Player2 { get => Seed2.Player; }

        public void HandleNewStatus(MatchStatus? newStatus)
        {
            if (newStatus == MatchStatus.Created) Score = null;
            else if (newStatus == MatchStatus.Finished) Venue = null;
            //else if (newStatus == MatchStatus.Finished)
            //{
            //    if(WinnerSeedFollowUp is object) WinnerSeedFollowUp.M
            //}
        }

        public static IEnumerable<Match> OrderMatches(IEnumerable<Match> matches)
        {
            return matches.OrderBy(m => m.MatchOrderValue);
        }

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
                stat.SetsLost += this.Score.P2Sets;
                stat.LegsLost += this.Score.P2Legs;
            } 
            else if (this.Seed2.Equals(s))
            {
                //Won
                stat.SetsWon += this.Score.P2Sets;
                stat.LegsWon += this.Score.P2Legs;

                //Lost
                stat.SetsLost += this.Score.P1Sets;
                stat.LegsLost += this.Score.P1Legs;
            } 
            else return false;

            //Update Winning Stat
            stat.Matches++;
            if (HasSeedWon(s))
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
                else if (this.Score.P1Legs < this.Score.P2Legs)
                {
                    //Seed 2 won
                    return s.Equals(this.Seed2) ? true : false;
                }
                return false;
            }
            else
            {
                //Check for Sets
                if (this.Score.P1Sets > this.Score.P2Sets)
                {
                    //Player 1 won
                    return s.Equals(this.Seed1) ? true : false;
                }
                else if (this.Score.P1Sets < this.Score.P2Sets)
                {
                    //Player 2 won
                    return s.Equals(this.Seed2) ? true : false;
                }
                return false;
            }
        }

        public bool IsMatchOfSeeds(Seed s1, Seed s2)
        {
            if (Seed1.Equals(s1) && Seed2.Equals(s2) || Seed1.Equals(s2) && Seed2.Equals(s1))
            {
                return true;
            }

            return false;
        }
    }

    public class MatchFactory
    {
        public static List<Match> CreateMatches(Round r)
        {
            List<Match> matches = new List<Match>();

            foreach (Group g in r.Groups)
            {
                //List<Match> tmpList = scheduleRoundRobin(g.Seeds.ToList());
                List<Match> tmpList = scheduleRoundRobinManual(g.Seeds.ToList());

                tmpList.ForEach(m => {
                    m.GroupId = g.GroupId; 
                    m.Status = Match.MatchStatus.Created;
                });  
                matches.AddRange(tmpList);
            }

            return matches;
        }

        private static List<Match> scheduleRoundRobinManual(List<Seed> seeds)
        {
            List<Match> matches = new List<Match>();
            if (seeds.Count == 3)
            {
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(1).SeedId, MatchOrderValue = 0 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(1).SeedId, Seed2Id = seeds.ElementAt(2).SeedId, MatchOrderValue = 1 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(2).SeedId, MatchOrderValue = 2 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(2).SeedId, Seed2Id = seeds.ElementAt(1).SeedId, MatchOrderValue = 3 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(2).SeedId, Seed2Id = seeds.ElementAt(0).SeedId, MatchOrderValue = 4 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(1).SeedId, Seed2Id = seeds.ElementAt(0).SeedId, MatchOrderValue = 5 });
            }
            else if (seeds.Count == 4)
            {
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(1).SeedId, MatchOrderValue = 1 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(2).SeedId, Seed2Id = seeds.ElementAt(3).SeedId, MatchOrderValue = 1 });

                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(2).SeedId, MatchOrderValue = 3 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(1).SeedId, Seed2Id = seeds.ElementAt(3).SeedId, MatchOrderValue = 3 });

                matches.Add(new Match() { Seed1Id = seeds.ElementAt(0).SeedId, Seed2Id = seeds.ElementAt(3).SeedId, MatchOrderValue = 5 });
                matches.Add(new Match() { Seed1Id = seeds.ElementAt(1).SeedId, Seed2Id = seeds.ElementAt(2).SeedId, MatchOrderValue = 5 });

            }
            return matches;
        }

        private static List<Match> scheduleRoundRobin(List<Seed> seeds)
        {
            List<Match> matches = new List<Match>();

            foreach(Tuple<int, int> pair in CreatePairs(seeds.Count))
            {
                matches.Add(new Match()
                {
                    Seed1Id = seeds.ElementAt(pair.Item1 - 1).SeedId,
                    Seed2Id = seeds.ElementAt(pair.Item2 - 1).SeedId,
                    MatchOrderValue = 0,
                });
            }

            return matches;
        }

        private static List<Tuple<int, int>> CreatePairs(int n)
        {
            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

            int[] orig = new int[n];
            for (int i = 0; i < n; i++)
            {
                orig[i] = i + 1;
            }
            IEnumerable<int> rev = orig.Reverse();

            int len = orig.Length;
            for (int j = 0; j < len - 1; j++)
            {
                List<int> tmp = new List<int>();
                tmp.Add(orig[0]);
                tmp.AddRange(rev.Take(j).Reverse());
                if (j < len && len > 1 + j) tmp.AddRange(orig.Skip(1).Take(len - 1 - j));
                pairs.AddRange(makeMatches(tmp, j + 1));
            }

            return pairs;
        }

        private static List<Tuple<int, int>> makeMatches(IEnumerable<int> arr, int round)
        {
            int halfSize = arr.Count() / 2;

            IEnumerable<int> A = arr.Take(halfSize);
            IEnumerable<int> B = arr.Skip(halfSize).Take(halfSize).Reverse();

            return A.Zip(B, (x, y) => new Tuple<int, int>(x,y)).ToList();
        }
    }
}
