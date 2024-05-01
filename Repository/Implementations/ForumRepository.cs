﻿using Microsoft.EntityFrameworkCore;
using ThreadShare.Data;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class ForumRepository : IForumRepository
    {
        private readonly AppDbContext _dbContext;

        public ForumRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Forum forum)
        {
            await _dbContext.Forums.AddAsync(forum);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int forumId)
        {
            var forumToDelete = await _dbContext.Forums.FindAsync(forumId);
            if (forumToDelete != null)
            {
                _dbContext.Forums.Remove(forumToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Implement this.
        public async Task Update(Forum forumToUpdate)
        {
            _dbContext.Entry(forumToUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Forum> GetById(int forumId)
        {
            return await _dbContext.Forums.FindAsync(forumId);
        }

        public async Task<List<Forum>> GetAllForums()
        {
            return await _dbContext.Forums.ToListAsync();
        }
    }
}
