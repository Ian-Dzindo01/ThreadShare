using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using ThreadShare.Data;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _dbContext;

        public PostRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int postId)
        {
            var postToDelete = await _dbContext.Posts.FindAsync(postId);
            if (postToDelete != null)
            {
                _dbContext.Posts.Remove(postToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(Post postToUpdate)
        {
            _dbContext.Entry(postToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Post> GetById(int postId)
        {
            return await _dbContext.Posts.FindAsync(postId);
        }

        public async Task<bool> InstanceExists(int id)
        {
            return await _dbContext.Posts.AnyAsync(f => f.Id == id);
        }

        // Load all of them in for now
        public async Task<IEnumerable<Post>> GetNewest()
        {
            var newestPosts = await _dbContext.Posts
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();

            return newestPosts;
        }
    }
}