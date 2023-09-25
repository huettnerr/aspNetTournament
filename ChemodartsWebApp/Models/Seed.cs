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
        [Column("ancestorMatchId")] public int? AncestorMatchId { get; set; }

        //Navigation
        [Display(Name = "Gruppe")] 
        public virtual Group Group { get; set; }
        public virtual MapTournamentSeedPlayer MappedTournamentPlayer { get; set; }
        public virtual Match? AncestorMatch { get; set; }
        public virtual ICollection<Match> MatchesAsS1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsS2 { get; set; } = new List<Match>();

        [NotMapped] public ICollection<Match> Matches { get { return MatchesAsS1.Concat(MatchesAsS2).ToList(); } }
        [NotMapped][Display(Name = "Spieler")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Player? Player { get => MappedTournamentPlayer?.Player; }
        [NotMapped][Display(Name = "Turnier")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Tournament Tournament { get => MappedTournamentPlayer.Tournament; }
        [NotMapped][Display(Name = "Stats")][DisplayFormat(NullDisplayText = "n. A.")] public virtual SeedStatistics Statistics { get; set; }

        public void UpdateSeedStatistics(int roundId)
        {
            SeedStatistics stats = new SeedStatistics(this.Group.Round.Scoring);
            //List<Match> relevantMatches = this.Matches.Where(m => (m.Player1?.Equals(this.Player) || m.Player2?.Equals(this.Player))).ToList();
            foreach (Match match in Matches.Where(m => m.Group.Round.RoundId == roundId))
            {
                match.UpdateSeedStat(this, stats);
            }
            Statistics = stats;
        }
    }

    public class SeedStatistics
    {
        [Display(Name = "Spiele")] public int Matches { get; set; }
        [Display(Name = "Siege")] public int MatchesWon { get; set; }
        [Display(Name = "Scheiße")] public int MatchesLost { get; set; }
        [Display(Name = "Tie")] public int MatchesTied { get; set; }
        [Display(Name = "Sets for")] public int SetsWon { get; set; }
        [Display(Name = "Sets against")] public int SetsLost { get; set; }
        [Display(Name = "Legs for")] public int LegsWon { get; set; }
        [Display(Name = "Legs against")] public int LegsLost { get; set; }

        [Display(Name = "Record")] public string Record { get => $"{MatchesWon} - {MatchesLost}"; }

        [Display(Name = "Legs Won")] 
        public int PointsFor
        { 
            get 
            { 
                switch(scoreType)
                {
                    case ScoreType.SetsOnly: return SetsWon;
                    case ScoreType.LegsOnly: return LegsWon;
                    default: return 0;
                }
            } 
        }
        
        [Display(Name = "Legs Lost")] 
        public int PointsAgainst
        { 
            get 
            { 
                switch(scoreType)
                {
                    case ScoreType.SetsOnly: return SetsLost;
                    case ScoreType.LegsOnly: return LegsLost;
                    default: return 0;
                }
            } 
        }

        [Display(Name = "Legs Diff.")]
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

        private ScoreType scoreType;  

        public SeedStatistics(ScoreType scoring)
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
