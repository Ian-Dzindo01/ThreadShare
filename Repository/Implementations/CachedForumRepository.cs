using Microsoft.Extensions.Caching.Memory;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class CachedForumRepository : IForumRepository
    {
        private readonly ForumRepository _decorated;
        private readonly IMemoryCache _memoryCache;

        public CachedForumRepository(ForumRepository decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public async Task Add(Forum forum)
        {
            await _decorated.Add(forum);
            _memoryCache.Remove("all-forums");
        }

        public async Task Delete(int forumId)
        {
            await _decorated.Delete(forumId);
            _memoryCache.Remove($"forum-{forumId}");
            _memoryCache.Remove("all-forums");
        }

        public async Task Update(Forum forumToUpdate)
        {
            await _decorated.Update(forumToUpdate);
            _memoryCache.Remove($"forum-{forumToUpdate.Id}");
            _memoryCache.Remove("all-forums");
        }

        public async Task<Forum> GetById(int forumId)
        {
            string cacheKey = $"forum-{forumId}";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    return await _decorated.GetById(forumId);
                });
        }

        public async Task<bool> InstanceExists(int forumId)
        {
            return await _decorated.InstanceExists(forumId);
        }

        public async Task<List<Forum>> GetAllForums()
        {
            string cacheKey = "all-forums";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return await _decorated.GetAllForums();
                });
        }

        public async Task<int?> GetForumIdByName(string name)
        {
            string cacheKey = $"forum-id-by-name-{name}";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return await _decorated.GetForumIdByName(name);
                });
        }
    }
}