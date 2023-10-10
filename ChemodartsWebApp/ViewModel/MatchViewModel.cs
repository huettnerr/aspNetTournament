using ChemodartsWebApp.ModelHelper;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class MatchViewModel : GroupViewModel
    {
        private Match? _m;
        public Match? M {
            get => _m;
            set
            {
                _m = value;
                if (_m is object) base.G = _m.Group;

                updateComparison();
            } 
        }
        public IEnumerable<Match> Ms { get; set; }
        public PlayerComparison? PC { get; private set; }

        private void updateComparison()
        {
            if(_m is object)
            {
                PC = new PlayerComparison(_m.Seed1?.Player, _m.Seed2?.Player);
            }
        }
    }
}
