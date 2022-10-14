using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ChemodartsWebApp.Models
{
    [Table("rounds")]
    public class Round
    {
        public enum RoundType
        {
            [Display(Name = "Gruppen")] RoundRobin,
            [Display(Name = "Einfach KO")] SingleKo,
            [Display(Name = "Doppel KO")] DoubleKo
        }

        [Key][Display(Name = "ID")][Column("roundId")] public int RoundId { get; set; }
        [Display(Name = "Turnier")][Column("tournamentId")] public int TournamentId { get; set; }
        [Display(Name = "Name")][Column("name")] public string? RoundName { get; set; }
        [Display(Name = "Typ")][Column("type")] public RoundType Type { get; set; }
    }
}
