using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("seeds")]
    public class Seed
    {
        [NotMapped] public static readonly Seed BYE_SEED = new Seed() { SeedName = "Bye" };
        public static Seed CreateByeSeed(int groupId) => new Seed() { SeedName = BYE_SEED.SeedName, GroupId = groupId };
        public bool IsByeSeed() => SeedName?.Equals(BYE_SEED.SeedName) ?? false;

        [Key][Column("seedId")] public int SeedId { get; set; }
        [Column("groupId")] public int GroupId { get; set; }
        [Display(Name = "Seed")][Column("seedNr")] public int SeedNr { get; set; }
        [Display(Name = "Rank")][Column("seedRank")] public int SeedRank { get; set; }
        [Display(Name = "Seed Name")][Column("seedName")] public string? SeedName { get; set; }
        [Display(Name = "Is Dummy?")][Column("isDummy")] public bool IsDummy { get; set; }

        //Navigation
        [Display(Name = "Gruppe")] public virtual Group Group { get; set; }
        [Display(Name = "Stats")][DisplayFormat(NullDisplayText = "n. A.")] public virtual SeedStatistics? SeedStatistics { get; set; } 
        public virtual MapRoundSeedPlayer MappedRoundSeedPlayer { get; set; }
        public virtual ICollection<Match> MatchesAsS1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsS2 { get; set; } = new List<Match>();

        [NotMapped] public ICollection<Match> Matches { get { return MatchesAsS1.Concat(MatchesAsS2).ToList(); } }
        //[NotMapped][Display(Name = "Turnier")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Tournament Tournament { get => MappedTournamentPlayer.Round.Tournament; }
        [NotMapped][Display(Name = "Runde")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Round Round { get => MappedRoundSeedPlayer?.Round; }
        [NotMapped][Display(Name = "Spieler")][DisplayFormat(NullDisplayText = "n. A.")] public virtual Player? Player { get => MappedRoundSeedPlayer?.Player; }
        [NotMapped] public virtual List<Seed?> PossibleSeeds { get; set; } = new List<Seed?>();

        public Seed() { }

        /// <summary>
        /// Clones the Seed without and Id and without mapped properties like statistics
        /// </summary>
        /// <returns></returns>
        //public Seed Clone()
        //{
        //    return new Seed()
        //    {
        //        GroupId = this.GroupId,
        //        SeedNr = this.SeedNr,
        //        SeedRank = this.SeedRank,
        //        SeedName = this.SeedName,
        //        IsDummy = this.IsDummy
        //    };
        //}

        public override string ToString()
        {
            return $"<s>[{SeedId}] \"{SeedName}\" ({Player?.ToString()})</s>";
        }
    }
}
