using ThreadShare.DTOs.Entites;
using ThreadShare.Models;

namespace ThreadShare.Service.Interfaces
{
    public interface ICommentService
    {
        public Task CreateComment(CommentViewModel comment);
        public Task UpdateComment(CommentViewModel model, int commentId);
        public Task DeleteComment(int commentId);
        public Task<Comment> GetCommentById(int commentId);
    }
}