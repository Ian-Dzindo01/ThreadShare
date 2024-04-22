using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThreadShare.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        //public int UserId { get; set; }

        //[ForeignKey("UserId")]
        //public User User { get; set; } // Navigation property

        //public int PostId { get; set; }

        //[ForeignKey("PostId")]
        //public Post Post { get; set; } // Navigation property

    }
}