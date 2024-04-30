using ThreadShare.Models;

namespace ThreadShare.Service.Interfaces
{
    public interface IPostService
    {
        void CreatePost(Post post);
        void UpdatePost(Post post);
        void DeletePost(int postId);
        Post GetPostById(int postId);
        IEnumerable<Post> GetAllPosts();
    }
}
