using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("venues")]
    public class Venue
    {
        [Key][Display(Name = "ID")][Column("venueId")] public int VenueId { get; set; }
        [Display(Name = "Ort")][Column("name")] public string? VenueName { get; set; }

        //Navigation
        public virtual Match Match { get; set; }
        public virtual ICollection<MapRoundVenue> MappedRounds { get; set; }
    }
}
