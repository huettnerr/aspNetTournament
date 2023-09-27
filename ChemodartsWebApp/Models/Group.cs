using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("groups")]
    public class Group
    {
        [Key][Display(Name = "ID")][Column("groupId")] public int GroupId { get; set; }
        [Required][Display(Name = "Gruppe")][Column("name")] public string? GroupName { get; set; }
        [Display(Name = "Rückspiel")][Column("hasRematch")] public bool GroupHasRematch { get; set; }

        //Navigation
        [Display(Name = "RoundID")][Column("roundId")] public int RoundId { get; set; }
        public virtual Round Round { get; set; }
        //public virtual ICollection<MapGroupPlayer> MappedPlayers { get; set; }
        [NotMapped] public virtual ICollection<Match> Matches { get; set; }
        [NotMapped] public virtual ICollection<Seed> Seeds { get ; set; }

        [NotMapped] public virtual ICollection<Seed> RankedSeeds {
            get
            {
                List<Seed> rankedSeeds = new List<Seed>();
                Seeds?.ToList().ForEach(s => {
                    s.UpdateSeedStatistics(RoundId);
                    rankedSeeds.Add(s);
                });
                rankedSeeds = rankedSeeds
                    .OrderByDescending(s => s.Statistics.MatchesWon)
                    .ThenByDescending(s => s.Statistics.PointsDiff)
                    .ThenByDescending(s => s.Statistics.PointsFor).ToList();

                //Direkter Vergleich
                for (int i = 0; i < rankedSeeds.Count - 1; i++)
                {
                    //Check if some seeds are totally identical
                    if (rankedSeeds[i].Statistics.MatchesWon == rankedSeeds[i + 1].Statistics.MatchesWon &&
                        rankedSeeds[i].Statistics.PointsDiff == rankedSeeds[i + 1].Statistics.PointsDiff &&
                        rankedSeeds[i].Statistics.PointsFor == rankedSeeds[i + 1].Statistics.PointsFor)
                    {
                        Match? m = Matches.Where(m => m.IsMatchOfSeeds(rankedSeeds[i], rankedSeeds[i + 1])).FirstOrDefault();
                        if (m is object)
                        {
                            if (rankedSeeds[i + 1].Equals(m.WinnerSeed))
                            {
                                Seed tmp = rankedSeeds[i];
                                rankedSeeds[i] = rankedSeeds[i + 1];
                                rankedSeeds[i + 1] = tmp;
                            }
                        }
                    }
                }
                return rankedSeeds;
            }
        }
        [NotMapped] public virtual ICollection<Match> OrderedMatches { get => Matches.OrderBy(m => m.MatchOrderValue).ToList(); }
    }
}
