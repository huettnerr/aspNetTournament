using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("map_tournament_seed_player")]
    public class MapTournamentSeedPlayer
    {
        [Key][Column("tpMapId")] public int TSP_Id { get; set; }
        [Column("tournamentId")] public int TSP_TournamentId { get; set; }
        [Display(Name = "Seed")][Column("seedId")] public int TSP_SeedId { get; set; }
        [Column("playerId")] public int? TSP_PlayerId { get; set; }
        [Display(Name = "Fixed?")][Column("playerFixed")] public bool TSP_PlayerFixed { get; set; }
        [Display(Name = "Checked In?")][Column("playerCheckedIn")] public bool TSP_PlayerCheckedIn { get; set; }

        //Navigation
        //public virtual ICollection<Player> Players { get; set; }
        [Display(Name = "ABC Gruppe")] public virtual Tournament Tournament { get; set; }
        [Display(Name = "ABC Seed")] public virtual Seed Seed { get; set; }
        [Display(Name = "ABC Spieler")] public virtual Player? Player { get; set; }
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
