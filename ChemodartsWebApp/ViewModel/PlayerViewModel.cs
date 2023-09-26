using ChemodartsWebApp.Models;
using ChemodartsWebApp.Data.Factory;

namespace ChemodartsWebApp.ViewModel
{
    public class PlayerViewModel : MainViewModel
    {
        public Player? P { get; set; }
        public IEnumerable<Player>? Ps { get; set; }
        public PlayerFactory? PF { get; set; }  

        public PlayerViewModel(Player? p) : base()
        {
            P = p;
            Ps = new List<Player>();
        }

        public PlayerViewModel(IEnumerable<Player> ps) : base() => Ps = ps;
        public PlayerViewModel(PlayerFactory? pf) : base() => PF = pf;
    }
}
