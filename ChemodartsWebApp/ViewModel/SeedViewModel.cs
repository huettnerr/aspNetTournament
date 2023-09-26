using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class SeedViewModel : TournamentViewModel
    {
        public Seed? S { get; set; }
        public IEnumerable<Seed>? Ss { get; set; }

        public SeedViewModel(Seed? s, Tournament? t) : base(t)
        {
            S = s;
            Ss = new List<Seed>();
        }

        public SeedViewModel(IEnumerable<Seed> ss, Tournament? t) : base(t) => Ss = ss;
    }
}
