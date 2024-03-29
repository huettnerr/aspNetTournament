﻿using ChemodartsWebApp.Data.Factory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("tournaments")]
    public class Tournament
    {
        [Key][Display(Name = "ID")][Column("tournamentId")] public int TournamentId { get; set; }
        [Required][Display(Name = "Turnier")][Column("name")] public string TournamentName { get; set; }
        [Display(Name = "Turnierbeginn")][DisplayFormat(NullDisplayText = "Nicht festgelegt")][DataType(DataType.DateTime)][Column("starttime")] public DateTime? TournamentStart { get; set; }

        //Navigation
        public virtual ICollection<Round>? Rounds { get; set; }
    }
}
