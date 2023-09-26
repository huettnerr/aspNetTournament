using ChemodartsWebApp.ModelHelper;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class MainViewModel
    {
        public Seed? S { get; set; }
        public Venue? V { get; set; }

        //public MainViewModel(Tournament? t = null, Player? p = null, Seed? s = null, Round? r = null, Group? g = null, Venue? v = null, Match? m = null)
        public MainViewModel()
        {
            S = null;
            V = null;
        }
    }
}
