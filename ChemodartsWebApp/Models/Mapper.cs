using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("map_group_player")]
    public class MapGroupPlayer
    {
        [Key][Column("gpMapId")] public int GPM_Id { get; set; }
        [Column("playerId")] public int GPM_PlayerId { get; set; }
        [Column("groupId")] public int GPM_GroupId { get; set; }
        [Display(Name = "Seed")][Column("seed")] public int GPM_Seed { get; set; }

        //Navigation
        //public virtual ICollection<Player> Players { get; set; }
        [Display(Name = "ABC Gruppe")] public virtual Group Group { get; set; }
        [Display(Name = "ABC Spieler")] public virtual Player Player { get; set; }
    }

    [Table("map_round_venue")]
    public class MapRoundVenue
    {
        [Key][Column("rvMapId")] public int RVM_Id { get; set; }
        [Column("roundId")] public int RVM_RoundId { get; set; }
        [Column("venueId")] public int RVM_VenueId { get; set; }

        //Navigation
        public virtual Round Round { get; set; }
        public virtual Venue Venue { get; set; }
    }
}
