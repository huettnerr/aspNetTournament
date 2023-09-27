using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChemodartsWebApp.ViewModel
{
    public class SeedViewModel : RoundViewModel
    {
        public Seed? S { get; set; }
        public IEnumerable<Seed>? Ss { get; set; }
        public MultiSelectList? Players { get; set; }
        public List<int>? SelectedPlayerIds { get; set; }

        public SeedViewModel() { } //Needed for POST
        public SeedViewModel(Round? r) : base(r) { }

        //public SeedViewModel(IEnumerable<Seed> ss, Round? r) : base(r) => Ss = ss;
        //public SeedViewModel(Round? r, IEnumerable<Player> asl) : base(r) => AddSeedList = asl;
    }
}
