using ThreadShare.Models;

namespace ThreadShare.Repository.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetById(int id);
        public Task Add(Comment comment);
        public Task Delete(int id);
        public Task Update(Comment comment);
    }
}