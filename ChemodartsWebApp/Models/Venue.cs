using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("venues")]
    public class Venue
    {
        [Key][Display(Name = "ID")][Column("venueId")] public int VenueId { get; set; }
        [Display(Name = "Name")][Column("name")] public string? VenueName { get; set; }
        [Display(Name = "Match")][Column("matchId")] public int MatchId { get; set; }
    }
}
