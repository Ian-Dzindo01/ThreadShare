using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ThreadShare.Data;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class CachedForumRepository : IForumRepository
    {
        private readonly ForumRepository _decorated;
        private readonly IDistributedCache _distributedCache;
        private readonly AppDbContext _dbContext;


        public CachedForumRepository(ForumRepository decorated, IDistributedCache distributedCache, AppDbContext dbContext)
        {
            _decorated = decorated;
            _distributedCache = distributedCache;
            _dbContext = dbContext;
        }

        public async Task Add(Forum forum)
        {
            await _decorated.Add(forum);
            _distributedCache.Remove("all-forums");
        }

        public async Task Delete(int forumId)
        {
            await _decorated.Delete(forumId);
            // Cache invalidation
            _distributedCache.Remove($"forum-{forumId}");
            _distributedCache.Remove("all-forums");
        }

        public async Task Update(Forum forumToUpdate)
        {
            await _decorated.Update(forumToUpdate);
            _distributedCache.Remove($"forum-{forumToUpdate.Id}");
            _distributedCache.Remove("all-forums");
        }

        public async Task<Forum> GetById(int id)
        {
            string key = $"forum-{id}";

            string cachedMember = await _distributedCache.GetStringAsync(
                key);

            Forum? forum;
            if (string.IsNullOrEmpty(cachedMember))
            {
                forum = await _decorated.GetById(id);

                if (forum is null)
                {
                    return forum;
                }

                await _distributedCache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(forum, new JsonSerializerSettings
                    {
                        // Fix infinite loop
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                );

                return forum;
            }

            forum = JsonConvert.DeserializeObject<Forum>(
                cachedMember);

            if (forum is not null)
            {
                _dbContext.Set<Forum>().Attach(forum);
            }

            return forum;
        }

        public async Task<bool> InstanceExists(int forumId)
        {
            return await _decorated.InstanceExists(forumId);
        }

        public async Task<List<Forum>> GetAllForums()
        {
            string cacheKey = "all-forums";

            string cachedForums = await _distributedCache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(cachedForums))
            {
                var forums = await _decorated.GetAllForums();

                if (forums == null || !forums.Any())
                {
                    return forums;
                }

                await _distributedCache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(forums, new JsonSerializerSettings
                    {
                        // Fix infinite loop
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });

                return forums;
            }

            // If cached data is available, deserialize and return it
            return JsonConvert.DeserializeObject<List<Forum>>(cachedForums);
        }


        public async Task<int?> GetForumIdByName(string name)
        {
            string cacheKey = $"forum-id-by-name-{name}";

            string cachedData = await _distributedCache.GetStringAsync(cacheKey);
            var forumId = await _decorated.GetForumIdByName(name);

            if (forumId.HasValue)
            {
                await _distributedCache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(forumId),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                return forumId;
            }

            return JsonConvert.DeserializeObject<int?>(cachedData);
        }
    }
}