using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("tournaments")]
    public class Tournament
    {
        [Key][Display(Name = "ID")][Column("tournamentId")] public int TournamentId { get; set; }
        [Display(Name = "Turniername")][Column("name")] public string? TournamentName { get; set; }
        [Display(Name = "Turnierbeginn")][DataType(DataType.DateTime)] [Column("starttime")] public DateTime? TournamentStart { get; set; }
    }
}
