using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("map_tournament_player")]
    public class MapTournamentPlayer
    {
        [Key][Column("tpMapId")] public int TPM_Id { get; set; }
        [Column("playerId")] public int TPM_PlayerId { get; set; }
        [Column("tournamentId")] public int TPM_TournamentId { get; set; }
        [Display(Name = "Seed")][Column("seedId")] public int TPM_SeedId { get; set; }
        [Display(Name = "Fixed")][Column("seedFixed")] public int TPM_SeedFixed { get; set; }

        //Navigation
        //public virtual ICollection<Player> Players { get; set; }
        [Display(Name = "ABC Gruppe")] public virtual Tournament Tournament { get; set; }
        [Display(Name = "ABC Spieler")] public virtual Player Player { get; set; }
        [Display(Name = "ABC Seed")] public virtual Seed Seed { get; set; }
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
