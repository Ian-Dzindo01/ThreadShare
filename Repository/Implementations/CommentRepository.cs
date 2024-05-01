﻿using Microsoft.EntityFrameworkCore;
using ThreadShare.Data;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Repository.Implementations
{
    public class CommentRepository :IRepository<Comment>
    {
        private readonly AppDbContext _dbContext;

        public CommentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int commentId)
        {
            var commentToDelete = await _dbContext.Comments.FindAsync(commentId);
            if (commentToDelete != null)
            {
                _dbContext.Comments.Remove(commentToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(Comment commentToUpdate)
        {
            _dbContext.Entry(commentToUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Comment> GetById(int commentId)
        {
            return await _dbContext.Comments.FindAsync(commentId);
        }
    }
}
