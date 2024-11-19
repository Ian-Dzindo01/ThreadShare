﻿using ThreadShare.Repository.Interfaces;
using ThreadShare.Models;
using Microsoft.Extensions.Caching.Memory;
namespace ThreadShare.Repository.Implementations
{
    public class CachedPostRepository : IPostRepository
    {

        private readonly PostRepository _decorated;
        private readonly IMemoryCache _memoryCache;

        public CachedPostRepository(PostRepository decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public async Task<Post> GetById(int id)
        {
            string key = $"member-{id}";

            return await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                    return await _decorated.GetById(id);
                });
        }

        public async Task Add(Post post)
        {
            await _decorated.Add(post);
        }

        public async Task Delete(int id)
        {
            await _decorated.Delete(id);
        }

        public async Task Update(Post post)
        {
            await _decorated.Update(post);
        }

        public async Task<IEnumerable<Post>> GetNewest()
        {
            return await _decorated.GetNewest();
        }
    }
}
