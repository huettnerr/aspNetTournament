using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Xml.Linq;
using static ChemodartsWebApp.Models.Match;

namespace ChemodartsWebApp.Models
{
    [Table("seed_statistics")]
    public class SeedStatistics
    {
        [Key][Column("seedStatisticsId")] public int SeedStatisticsId { get; set; }
        [Column("seedId")] public int? SeedId { get; set; }
        [Display(Name = "Spiele")][Column("matches")] public int Matches { get; set; }
        [Display(Name = "Siege")][Column("matchesWon")] public int MatchesWon { get; set; }
        [Display(Name = "Scheiße")][Column("matchesLost")] public int MatchesLost { get; set; }
        [Display(Name = "Tie")][Column("matchesTied")] public int MatchesTied { get; set; }
        [Display(Name = "Sets for")][Column("setsWon")] public int SetsWon { get; set; }
        [Display(Name = "Sets against")][Column("setsLost")] public int SetsLost { get; set; }
        [Display(Name = "Legs for")][Column("legsWon")] public int LegsWon { get; set; }
        [Display(Name = "Legs against")][Column("legsLost")] public int LegsLost { get; set; }

        //Navigation
        [Display(Name = "Seed")] public virtual Seed? Seed { get; set; }

        //Extended Stats
        private ScoreType scoreType = ScoreType.LegsOnly;
        [Display(Name = "Record")] public string Record { get => $"{MatchesWon} - {MatchesLost}"; }

        [Display(Name = "Points for")]
        public int PointsFor
        {
            get
            {
                switch (scoreType)
                {
                    case ScoreType.SetsOnly: return SetsWon;
                    case ScoreType.LegsOnly: return LegsWon;
                    default: return 0;
                }
            }
        }

        [Display(Name = "Points Against")]
        public int PointsAgainst
        {
            get
            {
                switch (scoreType)
                {
                    case ScoreType.SetsOnly: return SetsLost;
                    case ScoreType.LegsOnly: return LegsLost;
                    default: return 0;
                }
            }
        }

        [Display(Name = "Points Diff.")]
        public int PointsDiff
        {
            get
            {
                switch (scoreType)
                {
                    case ScoreType.SetsOnly: return SetsWon - SetsLost;
                    case ScoreType.LegsOnly: return LegsWon - LegsLost;
                    default: return 0;
                }
            }
        }

        public SeedStatistics()
        {
            ResetStatistic();
        }

        public void ResetStatistic()
        {
            Matches = 0;
            MatchesLost = 0;
            MatchesWon = 0;
            MatchesTied = 0;
            SetsWon = 0;
            SetsLost = 0;
            LegsWon = 0;
            LegsLost = 0;
        }

        public static SeedStatistics operator +(SeedStatistics ss1, SeedStatistics ss2)
        {
            ss1.MatchesWon += ss2.MatchesWon;
            ss1.MatchesLost += ss2.MatchesLost;
            ss1.MatchesTied += ss2.MatchesTied;
            ss1.SetsWon += ss2.SetsWon;
            ss1.SetsLost += ss2.SetsLost;
            ss1.LegsWon += ss2.LegsWon;
            ss1.LegsLost += ss2.LegsLost;
            return ss1;
        }
    }
}
