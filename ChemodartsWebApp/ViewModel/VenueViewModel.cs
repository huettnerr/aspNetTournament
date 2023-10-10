using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class VenueViewModel : RoundViewModel
    {
        public Venue? V { get; set; }
        public IEnumerable<Venue>? Vs { get; set; }
        public IEnumerable<Venue>? UnmappedVs { get; set; }
    }
}
