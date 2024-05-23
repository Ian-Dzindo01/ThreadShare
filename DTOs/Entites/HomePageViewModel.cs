using System.Collections.Generic;
using ThreadShare.Models;

namespace ThreadShare.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<Post> NewestPosts { get; set; }
        public IEnumerable<Forum> Forums { get; set; }
    }
}
