using ChemodartsWebApp.Models;
using ChemodartsWebApp.Data.Factory;

namespace ChemodartsWebApp.ViewModel
{
    public class TournamentViewModel : MainViewModel
    {
        public Tournament? T { get; set; }
        public TournamentFactory? TF { get; set; }

        public TournamentViewModel(Tournament? t) : base() => T = t;
        public TournamentViewModel(TournamentFactory? tf, Tournament? t) : base()
        {
            TF = tf;
            T = t;
        }
    }
}
