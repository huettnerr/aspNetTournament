using ChemodartsWebApp.ModelHelper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("players")]
    public class Player
    {
        [Key][Display(Name = "ID")][Column("playerId")] public int PlayerId { get; set; }
        [Display(Name = "Name")][Column("name")] public string? PlayerName { get; set; }
        [Display(Name = "Dartname")][Column("dartname")] public string? PlayerDartname { get; set; }
        [NotMapped][Display(Name = "Spieler")][DisplayFormat(NullDisplayText = "n. A.")] public string CombinedName { get => $"{PlayerName} \"{PlayerDartname}\""; }
        [Display(Name = "Kontakt")][Column("contactData")] public string? PlayerContactData { get; set; }
        [Display(Name = "Interpret")][Column("interpret")] public string? PlayerInterpret { get; set; }
        [Display(Name = "Einlaufsong")][Column("song")] public string? PlayerSong { get; set; }

        //Navigation
        public virtual ICollection<MapTournamentSeedPlayer>? MappedTournaments { get; set; }
        [NotMapped] public virtual ICollection<Seed>? Seeds { get => MappedTournaments?.Select(mtp => mtp.Seed).ToList(); }
        [NotMapped] public virtual ICollection<Match>? Matches { get => Seeds?.SelectMany(s => s.Matches).Distinct().ToList(); } 
    }

    public class PlayerFactory : FactoryBase<Player>
    {
        public override string Controller { get; } = "Players";

        public string? Name { get; set; }
        public string? Dartname { get; set; }
        public string? ContactData { get; set; }
        public string? Interpret { get; set; }
        public string? Song { get; set; }

        public PlayerFactory() { } //Needed for POST
        public PlayerFactory(string action, Player? p) : base(action) 
        { 
            if(p is object)
            {
                Name = p.PlayerName;
                Dartname = p.PlayerDartname;
                ContactData = p.PlayerContactData;
                Interpret = p.PlayerInterpret;
                Song = p.PlayerSong;
            }
        }

        public override Player? Create()
        {
            Player p = new Player();

            Update(ref p);

            return p;
        }

        public override void Update(ref Player p)
        {
            p.PlayerName = Name;
            p.PlayerDartname = Dartname;
            p.PlayerContactData = ContactData;
            p.PlayerInterpret = Interpret;
            p.PlayerSong = Song;
        }
    }
}
