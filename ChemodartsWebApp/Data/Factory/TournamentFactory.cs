using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.Data.Factory
{

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
