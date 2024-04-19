using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ThreadShare.Models
{
    public class User : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string Surname { get; set; }

        //public string Username { get; set; }

        //public string Email { get; set; }

        //public DateTime DateJoined { get; set; }

        //public User()
        //{
        //    DateJoined = DateTime.UtcNow;
        //}
    }
}

