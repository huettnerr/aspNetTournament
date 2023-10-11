using ChemodartsWebApp.Data.Factory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ChemodartsWebApp.Models
{
    public enum RoundModus
    {
        [Display(Name = "Gruppenphase")] RoundRobin,
        [Display(Name = "Einfach-KO")] SingleKo,
        [Display(Name = "Doppel-KO")] DoubleKo,
        [Display(Name = "Rangliste")] Ranking
    }

    public enum ScoreType
    {
        [Display(Name = "Default")] Default,
        [Display(Name = "Sets | Legs")] SetsAndLegs,
        [Display(Name = "Sets")] SetsOnly,
        [Display(Name = "Legs")] LegsOnly
    }

    [Table("rounds")]
    public class Round
    {
        [Key][Display(Name = "ID")][Column("roundId")] public int RoundId { get; set; }
        [Display(Name = "Turnier")][Column("tournamentId")] public int TournamentId { get; set; }
        [Column("roundOrderValue")] public int? RoundOrderValue { get; set; }
        [Display(Name = "Name")][Column("name")] public string RoundName { get; set; }
        [Display(Name = "Modus")][Column("modus")] public RoundModus Modus { get; set; }
        [Display(Name = "Typ")][Column("scoring")] public ScoreType Scoring { get; set; }
        [Display(Name = "Gestartet?")][Column("isStarted")] public bool IsRoundStarted { get; set; }
        [Display(Name = "Beendet?")][Column("isFinished")] public bool IsRoundFinished { get; set; }

        //Navigation
        public virtual Tournament Tournament { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }
        public virtual ICollection<MapRoundVenue>? MappedVenues { get; set; }
        public virtual ICollection<MapRoundSeedPlayer>? MappedSeedsPlayers { get; set; }
        public virtual ICollection<MapRoundProgression> ProgressionRulesAsBase { get; set; } = new List<MapRoundProgression>();
        public virtual ICollection<MapRoundProgression> ProgressionRulesAsTarget { get; set; } = new List<MapRoundProgression>();

        [NotMapped] public virtual ICollection<Seed>? Seeds { get => MappedSeedsPlayers?.Select(msp => msp.Seed).OrderBy(s => s.SeedNr).ToList(); }

    }
}
