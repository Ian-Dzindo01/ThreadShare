using Microsoft.Extensions.Caching.Memory;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class CachedUserRepository : IUserRepository
    {
        private readonly UserRepository _decorated;
        private readonly IMemoryCache _memoryCache;

        public CachedUserRepository(UserRepository decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public async Task<User> GetUserById(string userId)
        {
            string cacheKey = $"user-{userId}";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return await _decorated.GetUserById(userId);
                });
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            string cacheKey = "all-users";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return await _decorated.GetAllUsers();
                });
        }
    }
}
