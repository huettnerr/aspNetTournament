﻿using ChemodartsWebApp.ModelHelper;
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

    public class RoundFactory : FactoryBase<Round>
    {
        public override string Controller { get; } = "Round";

        public string Name { get; set; }
        public RoundModus RoundModus { get; set; }
        public ScoreType Scoring { get; set; }

        [ScaffoldColumn(false)]
        public Tournament? T { get; set; }

        public RoundFactory() { } //Needed for POST
        public RoundFactory(string action, Round r) : base(action)
        {
            if (r is object)
            {
                Name = r.RoundName;
                RoundModus = r.Modus;
                Scoring = r.Scoring;
            }
        }

        //public RoundFactory(string action, Tournament t) : base(action)
        //{
        //    Name = String.Empty;
        //    T = t;
        //}

        public override Round? Create()
        {
            if (T is null) return null;

            Round r = new Round();
            Update(ref r);
            r.TournamentId = T.TournamentId;

            return r;
        }

        public override void Update(ref Round r)
        {
            r.RoundName = Name;
            r.Modus = RoundModus;
            r.Scoring = Scoring.Equals(ScoreType.Default) ? ScoreType.LegsOnly : Scoring;
        }
    }
}
