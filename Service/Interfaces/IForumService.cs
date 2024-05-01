using ThreadShare.Models;
using ThreadShare.DTOs.Entites;

namespace ThreadShare.Service.Interfaces
{
    public interface IForumService
    {
        public Task CreateForum(ForumViewModel forum);
        public Task UpdateForum(ForumViewModel forum);
        public Task DeleteForum(int forumId);
        public Task<Forum> GetForumById(int forumId);
        public Task<List<Forum>> GetAllForums();
    }
}
