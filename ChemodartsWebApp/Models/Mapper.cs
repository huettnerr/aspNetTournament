using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("map_tournament_rounds")]
    public class MapperGroupsPlayers
    {
        [Key][Column("gpMapId")] public int GPMappingId { get; set; }
        [Column("playerId")] public int GPMappingPlayerId { get; set; }
        [Column("groupId")] public int GPMappingGroupId { get; set; }
        [Column("seed")] public int GPMappingSeed { get; set; }
    }

    [Table("map_rounds_venues")]
    public class MapperRoundsVenues
    {
        [Key][Column("rvMapId")] public int RVMappingId { get; set; }
        [Column("roundId")] public int RVMappingRoundId { get; set; }
        [Column("venueId")] public int RVMappingVenueId { get; set; }
    }
}
