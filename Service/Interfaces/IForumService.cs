using ThreadShare.Models;

namespace ThreadShare.Service.Interfaces
{
    public interface IForumService
    {
        public Task CreateForum(Forum forum);
        public Task UpdateForum(Forum forum);
        public Task DeleteForum(int forumId);
        public Task<Forum> GetForumById(int forumId);
        public Task<List<Forum>> GetAllForums();
    }
}
