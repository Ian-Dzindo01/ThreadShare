using ThreadShare.Models;

namespace ThreadShare.Service.Interfaces
{
    public interface IForumService
    {
        void CreateForum(Forum forum);
        void UpdateForum(Forum forum);
        void DeleteForum(int forumId);
        Forum GetForumById(int forumId);
        IEnumerable<Forum> GetAllForums();
    }
}
