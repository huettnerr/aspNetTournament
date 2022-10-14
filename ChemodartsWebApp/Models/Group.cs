using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    [Table("groups")]
    public class Group
    {
        [Key][Display(Name = "ID")][Column("groupId")] public int GroupId { get; set; }
        [Display(Name = "ID")][Column("roundId")] public int RoundId { get; set; }
        [Display(Name = "Name")][Column("name")] public string? GroupName { get; set; }
    }

}
