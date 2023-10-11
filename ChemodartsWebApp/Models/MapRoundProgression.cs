using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("map_round_progression")]
    public class MapRoundProgression
    {
        public enum TournamentProgressionType
        {
            [Display(Name = "Ranglistenpunkte")] PointsOnly,
            [Display(Name = "Gesetzt nach Platzierung")] RankingFixed,
            [Display(Name = "(Neu-)Auslosung")] RankingRandom
        }

        [Key][Column("mrpMapId")] public int TP_MrpMapId { get; set; }
        [Column("baseRoundId")] public int TP_BaseRoundId { get; set; }
        [Column("targetRoundId")] public int? TP_TargetRoundId { get; set; }
        [Display(Name = "Modus")][Column("progressionType")] public TournamentProgressionType ProgressionType { get; set; }
        [Display(Name = "Qualifikanten pro Gruppe")][Column("advancePlayerCount")] public int AdvanceCount { get; set; } = 0;
        [Display(Name = "Plätze mit Bye")][Column("byePlayerCount")] public int ByeCount { get; set; } = 0;

        //Navigation
        [Display(Name = "Runde")]
        public virtual Round BaseRound { get; set; }

        [Display(Name = "Zielrunde")]
        [DisplayFormat(NullDisplayText = "-")]
        public virtual Round? TargetRound { get; set; }
    }
}
