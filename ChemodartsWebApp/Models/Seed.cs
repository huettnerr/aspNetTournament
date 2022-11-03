using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("seeds")]
    public class Seed
    {
        [Key][Column("seedId")] public int SeedId { get; set; }
        [Column("groupId")] public int GroupId { get; set; }
        [Display(Name = "Seed")][Column("seedNr")] public int SeedNr { get; set; }
        [Display(Name = "Seed Name")][Column("seedName")] public string? SeedName { get; set; }

        //Navigation
        [Display(Name = "Gruppe")] 
        public virtual Group Group { get; set; }
        public virtual MapTournamentSeedPlayer MappedTournamentPlayer { get; set; }
        public virtual ICollection<Match> MatchesAsS1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsS2 { get; set; } = new List<Match>();

        [NotMapped] public ICollection<Match> Matches { get { return MatchesAsS1.Concat(MatchesAsS2).ToList(); } }
        [NotMapped][Display(Name = "Spieler")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Player? Player { get => MappedTournamentPlayer?.Player; }
        [NotMapped][Display(Name = "Turnier")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Tournament Tournament { get => MappedTournamentPlayer.Tournament; }
        [NotMapped][Display(Name = "Stats")][DisplayFormat(NullDisplayText = "n. A.")] public virtual SeedStatistics Statistics { get => this.getSeedStatistics(); }

        private SeedStatistics getSeedStatistics()
        {
            SeedStatistics stats = new SeedStatistics(this.Group.Round.Scoring);
            //List<Match> relevantMatches = this.Matches.Where(m => (m.Player1?.Equals(this.Player) || m.Player2?.Equals(this.Player))).ToList();
            foreach(Match match in Matches)
            {
                match.UpdateSeedStat(this, stats);
            }
            return stats;
        }

    }

    public class SeedStatistics
    {
        [Display(Name = "W")] public int MatchesWon { get; set; }
        [Display(Name = "L")] public int MatchesLost { get; set; }
        [Display(Name = "L")] public int MatchesTied { get; set; }
        [Display(Name = "Sets for")] public int SetsWon { get; set; }
        [Display(Name = "Sets against")] public int SetsLost { get; set; }
        [Display(Name = "Legs for")] public int LegsWon { get; set; }
        [Display(Name = "Legs against")] public int LegsLost { get; set; }

        [Display(Name = "Record")] public string Record { get => $"{MatchesWon} - {MatchesLost}"; }

        [Display(Name = "Pts")] 
        public int Points 
        { 
            get 
            { 
                switch(scoreType)
                {
                    case Round.ScoreType.SetsOnly: return SetsWon;
                    case Round.ScoreType.LegsOnly: return LegsWon;
                    default: return 0;
                }
            } 
        }

        [Display(Name = "Pts Diff")]
        public int PointsDiff
        {
            get
            {
                switch (scoreType)
                {
                    case Round.ScoreType.SetsOnly: return SetsWon - SetsLost;
                    case Round.ScoreType.LegsOnly: return LegsWon - LegsLost;
                    default: return 0;
                }
            }
        }

        private Round.ScoreType scoreType;  

        public SeedStatistics(Round.ScoreType scoring)
        {
            scoreType = scoring;

            MatchesLost = 0;
            MatchesWon = 0;
            MatchesTied = 0;
            SetsWon = 0;
            SetsLost = 0;
            LegsWon = 0;
            LegsLost = 0;
        }
    }
}
