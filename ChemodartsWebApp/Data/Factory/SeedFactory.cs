using ChemodartsWebApp.Models;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace ChemodartsWebApp.Data.Factory
{
    public class SeedFactory : FactoryBase<Seed>
    {
        public override string Controller { get; } = "Group";
        public string Name { get; set; }
        public int Nr { get; set; }
        public int Rank { get; set; }

        public int? AnchestorMatchId { get; set; }

        [ScaffoldColumn(false)]
        public Group? G { get; set; }

        public SeedFactory() { } //Needed for POST
        public SeedFactory(string action, Seed? s) : base(action)
        {
            if (s is object)
            {
                Name = s.SeedName;
                Nr = s.SeedNr;
                Rank = s.SeedRank;
                AnchestorMatchId = s.AncestorMatchId;
            }
            else
            {
                Name = String.Empty;
                Nr = 0;
                Rank = 0;
                AnchestorMatchId = null;
            }
        }

        public override Seed? Create()
        {
            if (G is null) return null;

            Seed s = new Seed();
            s.SeedStatistics = new SeedStatistics();
            s.GroupId = G.GroupId;

            Update(ref s);

            return s;
        }

        public override void Update(ref Seed s)
        {
            s.SeedName = Name;
            s.SeedNr = Nr;
            s.SeedRank = Rank;
        }
    }
}
