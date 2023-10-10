using ChemodartsWebApp.Models;
using ChemodartsWebApp.Data.Factory;

namespace ChemodartsWebApp.ViewModel
{
    public class PlayerViewModel : MainViewModel
    {
        public Player? P { get; set; }
        public IEnumerable<Player>? Ps { get; set; }
        public PlayerFactory? PF { get; set; }  
    }
}
