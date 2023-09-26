using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class VenueViewModel : RoundViewModel
    {
        public Venue? V { get; set; }
        public IEnumerable<Venue>? Vs { get; set; }
        public IEnumerable<Venue>? UnmappedVs { get; set; }

        public VenueViewModel(Venue? v, Round? r) : base(r)
        {
            V = v;
            Vs = new List<Venue>();
        }

        public VenueViewModel(IEnumerable<Venue> vs, IEnumerable<Venue> umappedVs, Round? r) : base(r)
        {
            Vs = vs;
            UnmappedVs = umappedVs;
        }
    }
}
