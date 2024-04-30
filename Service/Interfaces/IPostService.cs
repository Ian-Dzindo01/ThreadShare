using ThreadShare.Models;

namespace ThreadShare.Service.Interfaces
{
    public interface IPostService
    {
        public Task CreatePost(Post post);
        public Task UpdatePost(Post post);
        public Task DeletePost(int postId);
        public Task<Post> GetPostById(int postId);
        public Task<List<Post>> GetAllPosts();
    }
}
