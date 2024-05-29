using ThreadShare.Models;

namespace ThreadShare.DTOs.Entites
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public Forum Forum { get; set; }
        // Change
        public List<Comment> Comments { get; set; }
        public string Username { get; set; }
    }
}
