using System.ComponentModel.DataAnnotations.Schema;

namespace ThreadShare.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; } 

        public int ForumId { get; set; }

        [ForeignKey("ForumId")]
        public Forum Forum { get; set; } 

        public DateTime DateCreated { get; set; }

        public Comment()
        {
            DateCreated = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
