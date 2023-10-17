using ChemodartsWebApp.Models;
using ChemodartsWebApp.Data.Factory;

namespace ChemodartsWebApp.ViewModel
{
    public class RoundViewModel : TournamentViewModel
    {
        private Round? _r;
        public Round? R { 
            get => _r;
            set
            {
                _r = value;
                if (_r is object)
                {
                    base.T = _r.Tournament;
                }
            } 
        }

        public RoundFactory? RF { get; set; }
    }
}
