using ThreadShare.Models;
using ThreadShare.Data;
using ThreadShare.Service.Interfaces;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Service.Implementations
{
    public class ForumService : IForumService
    {
        private readonly IForumRepository _forumRepository;

        public ForumService(IForumRepository forumRepository)
        {
            _forumRepository = forumRepository;
        }

        public async Task CreateForum(Forum forum)
        {

            _forumRepository.Add(forum);
        }

        public async Task UpdateForum(Forum forum)
        {

            _forumRepository.Update(f)
        }

        public async Task DeleteForum(int forumId)
        {

        }

        public async Task<Forum> GetForumById(int forumId)
        {


        }

        public async Task<List<Forum>> GetAllForums()
        { 

        }
    }
}
