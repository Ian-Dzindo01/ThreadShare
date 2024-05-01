using System.Runtime.InteropServices.Marshalling;
using ThreadShare.DTOs.Entites;
using ThreadShare.Models;
using ThreadShare.Repository.Implementations;
using ThreadShare.Repository.Interfaces;
using ThreadShare.Service.Interfaces;

namespace ThreadShare.Service.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly CommentRepository _commentRepository;

        public CommentService(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task CreateComment(CommentViewModel model)
        {
            var comment = new Comment
            {
                Body = model.Body,
                UserId = model.UserId,
                ForumId = model.ForumId,
                PostId = model.PostId
            };

            await _commentRepository.Add(comment);
        }

        public async Task UpdateComment(CommentViewModel model, int commentId)
        {
            Comment existingComment = await _commentRepository.GetById(commentId);

            if (existingComment != null)
            {
                existingComment.Body = model.Body;

                await _commentRepository.Update(existingComment);
            }
            else
            {
                throw new ArgumentException("Comment not found.");
            }
        }

        public async Task DeleteComment(int commentId)
        {
            await _commentRepository.Delete(commentId);
        }

        public async Task<Comment> GetCommentById(int commentId)
        {
            return await _commentRepository.GetById(commentId);
        }
    }
}
