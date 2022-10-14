using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("scores")]
    public class Score
    {
        [Key][Display(Name = "ID")][Column("scoreId")] public int ScoreId { get; set; }
        [Display(Name = "Set Heim")][Column("p1Sets")] public int P1Sets { get; set; }
        [Display(Name = "Set Gast")][Column("p2Sets")] public int P2Sets { get; set; }
        [Display(Name = "Leg Heim")][Column("p1Legs")] public int P1Legs { get; set; }
        [Display(Name = "Leg Gast")][Column("p2Legs")] public int P2Legs { get; set; }

        //View Objects
        [NotMapped][Display(Name = "Sets")] public string Sets { get { return $"{P1Sets} : {P2Sets}"; } }
        [NotMapped][Display(Name = "Legs")]public string Legs { get { return $"{P1Legs} : {P2Legs}"; } }

        //Navigation
        [Display(Name = "Heim")][Column("matchId")] public int MatchId { get; set; }
        public virtual Match Match { get; set; }
    }
}
