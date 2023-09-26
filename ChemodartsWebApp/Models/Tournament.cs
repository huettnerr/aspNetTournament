using ChemodartsWebApp.ModelHelper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("tournaments")]
    public class Tournament
    {
        [Key][Display(Name = "ID")][Column("tournamentId")] public int TournamentId { get; set; }
        [Required][Display(Name = "Turnier")][Column("name")] public string TournamentName { get; set; }
        [Display(Name = "Turnierbeginn")][DisplayFormat(NullDisplayText = "Nicht festgelegt")][DataType(DataType.DateTime)][Column("starttime")] public DateTime? TournamentStart { get; set; }

        //Navigation
        public virtual ICollection<Round> Rounds { get; set; }
        public virtual ICollection<MapTournamentSeedPlayer> MappedSeedsPlayers { get; set; }
        [NotMapped] public virtual ICollection<Seed> Seeds { get => MappedSeedsPlayers.Select(msp => msp.Seed).ToList(); }
    }

    public class TournamentFactory : FactoryBase<Tournament>
    {
        public override string Controller { get; } = "Tournament";

        public string Name { get; set; }
        public DateTime? StartTime { get; set; }

        public TournamentFactory() { } //Needed for POST
        public TournamentFactory(string action, Tournament? t) : base(action)
        {
            if (t is object)
            {
                Name = t.TournamentName;
                StartTime = t.TournamentStart;
            }
            else
            {
                Name = String.Empty;
            }
        }

        public override Tournament? Create()
        {
            Tournament t = new Tournament();

            Update(ref t);

            return t;
        }

        public override void Update(ref Tournament t)
        {
            t.TournamentName = Name;
            t.TournamentStart = StartTime;
        }
    }
}
