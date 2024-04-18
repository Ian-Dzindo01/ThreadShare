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

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string password { get; set; }

        public string Email { get; set; }

        public DateTime DateJoined { get; set; }

        public User()
        {
            DateJoined = DateTime.UtcNow;
        }
    }
}

