using ThreadShare.DTOs.Entites;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;
using ThreadShare.Service.Interfaces;

namespace ThreadShare.Service.Implementations
{
    public class ForumService : IForumService
    {
        private readonly IForumRepository _forumRepository;

        public ForumService(IForumRepository forumRepository)
        {
            _forumRepository = forumRepository;
        }

        public async Task CreateForum(ForumViewModel model)
        {
            var forum = new Forum
            {
                Name = model.Name,
                Description = model.Description,
                UserId = model.UserId,
            };

            await _forumRepository.Add(forum);
        }

        public async Task UpdateForum(ForumViewModel model, int forumId)
        {
            Forum existingForum = await _forumRepository.GetById(forumId);

            if (existingForum != null)
            {
                existingForum.Name = model.Name;
                existingForum.Description = model.Description;

                await _forumRepository.Update(existingForum);
            }
            else
            {
                throw new ArgumentException("Forum not found.");
            }
        }

        public async Task DeleteForum(int forumId)
        {
            await _forumRepository.Delete(forumId);
        }

        public async Task<Forum> GetForumById(int forumId)
        {
            return await _forumRepository.GetById(forumId);
        }

        public async Task<bool> ForumExists(int forumId)
        {
            return await _forumRepository.InstanceExists(forumId);
        }

        public async Task<int?> GetForumIdByName(string name)
        {
            return await _forumRepository.GetForumIdByName(name);
        }

        public async Task<List<Forum>> GetAllForums()
        {
            return await _forumRepository.GetAllForums();
        }
    }
}