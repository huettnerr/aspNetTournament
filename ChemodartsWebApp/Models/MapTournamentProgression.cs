using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("map_tournament_progression")]
    public class MapTournamentProgression
    {
        public enum TournamentProgressionType
        {
            [Display(Name = "Ranglistenpunkte")] PointsOnly,
            [Display(Name = "Gesetzt nach Platzierung")] RankingFixed,
            [Display(Name = "(Neu-)Auslosung")] RankingRandom
        }

        [Key][Column("tournamentId")] public int TP_TournamentId { get; set; }
        [Key][Column("roundId")] public int TP_RoundId { get; set; }
        [Display(Name = "Modus")][Column("progressionType")] public TournamentProgressionType ProgressionType { get; set; }
        [Display(Name = "Qualifikanten pro Gruppe")][Column("advanceCount")] public int AdvanceCount { get; set; } = 0;
        [Display(Name = "Plätze mit Bye")][Column("byeRoundCount")] public int ByeRoundCount { get; set; } = 0;

        //Navigation
        public virtual Tournament Tournament { get; set; }
        public virtual Round Round { get; set; }
    }
}
