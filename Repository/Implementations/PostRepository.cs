using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using ThreadShare.Data;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class PostRepository : IRepository<Post>
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

        //public async Task<List<Post>> GetAllPosts()
        //{
        //    return await _dbContext.Posts.ToListAsync();
        //}
    }
}