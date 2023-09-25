using ChemodartsWebApp.ModelHelper;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class MatchViewModel : RoundViewModel
    {
        private Match? _m;
        public Match? M {
            get => _m;
            set
            {
                _m= value;
                updateComparison(value);
            } 
        }
        public IEnumerable<Match> Ms { get; set; }
        public PlayerComparison? PC { get; private set; }

        public MatchViewModel(Match? m) : base(m?.Group.Round)
        {
            M = m;
            Ms = new List<Match>();
        }

        public MatchViewModel(Round? round, IEnumerable<Match> matches, Match? m = null) : base(round)
        {
            M = m;  
            Ms = matches;
        }

        private void updateComparison(Match? m)
        {
            if(m is object)
            {
                PC = new PlayerComparison(m.Seed1.Player, m.Seed2.Player);
            }
        }
    }
}
