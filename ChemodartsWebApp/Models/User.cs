using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemodartsWebApp.Models
{
    public class User
    {
        public const string Position = "Users";
        public static readonly User Admin = new User() { UserName = "rotateAdmin", Password = "chem69" };

        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
