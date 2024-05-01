using ThreadShare.Models;

namespace ThreadShare.Repository.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetById(int id);
        public Task Add(Post post);
        public Task Delete(int id);
        public Task Update(Post post);
        public Task<List<Post>> GetAllPosts();
    }
}