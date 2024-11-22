using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ThreadShare.Data;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class CachedPostRepository : IPostRepository
    {

        private readonly PostRepository _decorated;
        private readonly IDistributedCache _distributedCache;
        private readonly AppDbContext _dbContext;

        public CachedPostRepository(PostRepository decorated, IDistributedCache distributedCache, AppDbContext dbContext)
        {
            _decorated = decorated;
            _distributedCache = distributedCache;
            _dbContext = dbContext;
        }

        public async Task<Post> GetById(int id)
        {
            string key = $"post-{id}";

            string cachedMember = await _distributedCache.GetStringAsync(
                key);

            Post? post;
            if (string.IsNullOrEmpty(cachedMember))
            {
                post = await _decorated.GetById(id);

                if (post is null)
                {
                    return post;
                }

                await _distributedCache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(post));

                return post;
            }

            post = JsonConvert.DeserializeObject<Post>(
                cachedMember);

            if (post is not null)
            {
                _dbContext.Set<Post>().Attach(post);
            }

            return post;
        }

        public async Task Delete(int postId)
        {
            string cacheKey = $"post-{postId}";
            await _decorated.Delete(postId);
            await _distributedCache.RemoveAsync(cacheKey);
        }


        public async Task Update(Post post)
        {
            _decorated.Update(post);
            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            string cacheKey = $"post-{post.Id}";
            await _distributedCache.RemoveAsync(cacheKey);
        }

        // Invalidate GetNewest
        public async Task Add(Post post)
        {
            string cacheKey = "newest-posts";
            _decorated.Add(post);

            // Invalidate GetNewest
            await _distributedCache.RemoveAsync(cacheKey);
        }

        public async Task<IEnumerable<Post>> GetNewest()
        {
            string cacheKey = "newest-posts";

            string cachedPosts = await _distributedCache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(cachedPosts))
            {
                var posts = await _decorated.GetNewest();

                if (posts == null || !posts.Any())
                {
                    return posts;
                }

                await _distributedCache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(posts),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                    });

                return posts;
            }

            return JsonConvert.DeserializeObject<IEnumerable<Post>>(cachedPosts);
        }
    }
}

