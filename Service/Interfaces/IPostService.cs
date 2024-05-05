using ThreadShare.Models;
using ThreadShare.DTOs.Entites;

namespace ThreadShare.Service.Interfaces
{
    public interface IPostService
    {
        public Task CreatePost(PostViewModel post);
        public Task UpdatePost(PostViewModel post, int postId);
        public Task DeletePost(int postId);
        public Task<Post> GetPostById(int postId);
        //public Task<List<Post>> GetAllPosts();
    }
}
