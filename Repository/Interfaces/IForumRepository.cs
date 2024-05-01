using ThreadShare.Models;
using ThreadShare.DTOs.Entites;

namespace ThreadShare.Repository.Interfaces
{
    public interface IForumRepository
    {
        Task<Forum> GetById(int id);
        public Task Add(Forum forum);
        public Task Delete(int id);
        public Task Update(Forum forum);
        public Task<List<Forum>> GetAllForums();
    }
}