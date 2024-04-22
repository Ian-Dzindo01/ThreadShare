namespace ThreadShare.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }

        //public int UserId { get; set; }

        //[ForeignKey("UserId")]
        //public User User { get; set; } // Navigation property

        //public int PostId { get; set; }

        //[ForeignKey("PostId")]
        //public Post Post { get; set; } // Navigation property

        //public int ForumId { get; set; }

        //[ForeignKey("ForumId")]
        //public Forum Forum { get; set; } // Navigation property

    }
}
