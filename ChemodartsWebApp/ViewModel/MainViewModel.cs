using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class MainViewModel
    {
        public Player? P { get; set; }
        public Seed? S { get; set; }
        public Group? G { get; set; }
        public Venue? V { get; set; }
        public Match? M { get; set; }

        //public MainViewModel(Tournament? t = null, Player? p = null, Seed? s = null, Round? r = null, Group? g = null, Venue? v = null, Match? m = null)
        public MainViewModel()
        {
            P = null;
            S = null;
            G = null;
            V = null;
            M = null;
        }
    }
}
