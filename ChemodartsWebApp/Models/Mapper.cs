using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("map_round_seed_player")]
    public class MapRoundSeedPlayer
    {
        [Key][Column("tpMapId")] public int TSP_Id { get; set; }
        [Column("roundId")] public int TSP_RoundId { get; set; }
        [Display(Name = "Seed")][Column("seedId")] public int TSP_SeedId { get; set; }
        [Column("playerId")] public int? TSP_PlayerId { get; private set; }
        [Display(Name = "Fixed?")][Column("playerFixed")] public bool TSP_PlayerFixed { get; set; }
        [Display(Name = "Checked In?")][Column("playerCheckedIn")] public bool TSP_PlayerCheckedIn { get; set; }

        //Navigation
        [Display(Name = "ABC Gruppe")] public virtual Round Round { get; set; }
        [Display(Name = "ABC Seed")] public virtual Seed Seed { get; set; }
        [Display(Name = "ABC Spieler")] public virtual Player? Player { get; private set; }

        public MapRoundSeedPlayer() { }

        //Cloning Constructor
        public MapRoundSeedPlayer(MapRoundSeedPlayer mrsp)
        {
            TSP_PlayerCheckedIn = mrsp.TSP_PlayerCheckedIn;
            TSP_PlayerFixed = mrsp.TSP_PlayerFixed;
            Round = mrsp.Round;
            Seed = mrsp.Seed;
            SetPlayer(mrsp.Player);
        }

        public void SetPlayer(Player? p)
        {
            TSP_PlayerId = p?.PlayerId;
            Player = p;
            if(Seed is object) Seed.SeedName = p?.ShortName ?? "";
        }

        public override string ToString()
        {
            return $"[{TSP_Id}] {Seed.ToString()} mit {Player?.ToString()}";
        }
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
