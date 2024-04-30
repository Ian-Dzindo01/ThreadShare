using ThreadShare.Models;

namespace ThreadShare.Repository.Interfaces
{
    public interface IForumRepository
    {
        Task<Forum> GetById(int id);
        public Task Add(Forum forum);
        public Task Delete(int id);
        public Task Update(int id);
        public Task<List<Forum>> GetAllForums();
    }
}
