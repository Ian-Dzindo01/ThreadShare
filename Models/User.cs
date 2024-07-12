using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ThreadShare.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }

        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Surname { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Username { get; set; }

        public DateTime DateJoined { get; set; }

        // One to many relationship with Post class
        public ICollection<Post> Posts { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }

        public User()
        {
            DateJoined = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

    }
}

