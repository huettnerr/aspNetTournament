using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class HomeViewModel : MainViewModel
    {
        public IEnumerable<Tournament>? Ts { get; set; }

        public HomeViewModel() : base() => Ts = new List<Tournament>();
        public HomeViewModel(IEnumerable<Tournament> ts) : base() => Ts = ts;
    }
}
