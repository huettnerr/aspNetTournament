using ChemodartsWebApp.Models;
using ChemodartsWebApp.Data.Factory;

namespace ChemodartsWebApp.ViewModel
{
    public class TournamentViewModel : MainViewModel
    {
        public Tournament? T { get; set; }
        public IEnumerable<Tournament>? Ts { get; set; }
        public TournamentFactory? TF { get; set; }
    }
}
