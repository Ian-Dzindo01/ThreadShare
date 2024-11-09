using System.ComponentModel.DataAnnotations;
using ThreadShare.DTOs.Entites;

namespace ThreadShare.Models
{
    public class CreatePostViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The body cannot exceed 5000 characters.")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Please select a forum.")]
        public int SelectedForumId { get; set; }

        public List<ForumViewModel> Forums { get; set; } = new List<ForumViewModel>();
    }
}