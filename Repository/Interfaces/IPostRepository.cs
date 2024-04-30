using ThreadShare.Models;

namespace ThreadShare.Repository.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetById(int id);
        public Task Add(Post post);
        public Task Delete(int id);
        public Task Update(int id);
        public Task<List<Post>> GetAllPosts();
    }
}
