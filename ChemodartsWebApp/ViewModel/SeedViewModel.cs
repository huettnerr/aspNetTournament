using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChemodartsWebApp.ViewModel
{
    public class SeedViewModel : RoundViewModel
    {
        private Seed? _s;
        public Seed? S
        {
            get => _s;
            set
            {
                _s = value;
                if (_s != null) base.R = _s.Round;
            }
        }

        public IEnumerable<Seed>? Ss { get; set; }
        public MultiSelectList? Players { get; set; }
        public List<int>? SelectedPlayerIds { get; set; }
    }
}
