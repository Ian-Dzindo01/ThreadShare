using ThreadShare.Models;

namespace ThreadShare.Repository.Interfaces
{
    public interface IPostRepository
    {
        Forum GetById(int id);
        public void Add(Post post);
        public void Delete(int id);
        public void Update(int id);
    }
}
