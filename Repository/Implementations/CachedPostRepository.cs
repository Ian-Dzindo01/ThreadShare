﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class CachedPostRepository : IPostRepository
    {

        private readonly PostRepository _decorated;
        private readonly IDistributedCache _distributedCache;

        public CachedPostRepository(PostRepository decorated, IDistributedCache distributedCache)
        {
            _decorated = decorated;
            _distributedCache = distributedCache;
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

            //new JsonSerializerSettings
            //{
            //    ConstructorHandling =
            //        ConstructorHandling.AllowNonPublicDefaultConstructor
            //}

            return post;
        }

        public async Task Add(Post post)
        {
            await _decorated.Add(post);
            //_memoryCache.Remove("newest-posts");    // Invalidating cache
        }

        public async Task Delete(int id)
        {
            await _decorated.Delete(id);
            //_memoryCache.Remove("newest-posts");
        }

        public async Task Update(Post post)
        {
            await _decorated.Update(post);
            //_memoryCache.Remove("newest-posts");
        }

        public async Task<IEnumerable<Post>> GetNewest()
        {

            return await _decorated.GetNewest();

            //string cacheKey = "newest-posts";

            //return await _memoryCache.GetOrCreateAsync(
            //    cacheKey,
            //    async entry =>
            //    {
            //        entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            //        return await _decorated.GetNewest();
            //    });
        }
    }
}
