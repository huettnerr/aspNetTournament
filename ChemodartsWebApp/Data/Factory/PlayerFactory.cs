using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.Data.Factory
{
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
            if (p is object)
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
