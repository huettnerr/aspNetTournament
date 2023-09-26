using ChemodartsWebApp.Models;
using ChemodartsWebApp.Data.Factory;

namespace ChemodartsWebApp.ViewModel
{
    public class RoundViewModel : TournamentViewModel
    {
        public Round? R { get; set; }
        public RoundFactory? RF { get; set; }

        public RoundViewModel(Round? r) : base(r?.Tournament)
        {
            R = r;
        }

        public RoundViewModel(Tournament tournament, RoundFactory rf) : base(tournament) => RF = rf;
    }
}
