﻿using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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

        // Implement this
        public async Task Update(int postId)
        {
            var postToUpdate = await _dbContext.Forums.FindAsync(postId);
            _dbContext.Entry(postToUpdate).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Post> GetById(int postId)
        {
            return await _dbContext.Posts.FindAsync(postId);
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _dbContext.Posts.ToListAsync();
        }
    }
}