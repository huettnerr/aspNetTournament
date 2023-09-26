﻿using ChemodartsWebApp.Data.Factory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ChemodartsWebApp.Models
{
    public enum RoundModus
    {
        [Display(Name = "Gruppenphase")] RoundRobin,
        [Display(Name = "Einfach KO")] SingleKo,
        [Display(Name = "Doppel KO")] DoubleKo
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
        [Display(Name = "Name")][Column("name")] public string RoundName { get; set; }
        [Display(Name = "Typ")][Column("modus")] public RoundModus Modus { get; set; }
        [Display(Name = "Typ")][Column("scoring")] public ScoreType Scoring { get; set; }

        //Navigation
        public virtual Tournament Tournament { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<MapRoundVenue> MappedVenues { get; set; }

    }
}
