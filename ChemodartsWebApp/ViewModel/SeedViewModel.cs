using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class SeedViewModel : RoundViewModel
    {
        public Seed? S { get; set; }
        public IEnumerable<Seed>? Ss { get; set; }

        public SeedViewModel(Seed? s, Round? r) : base(r)
        {
            S = s;
            Ss = new List<Seed>();
        }

        public SeedViewModel(IEnumerable<Seed> ss, Round? r) : base(r) => Ss = ss;
    }
}
