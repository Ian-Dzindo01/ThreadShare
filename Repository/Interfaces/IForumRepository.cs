using ThreadShare.Models;

namespace ThreadShare.Repository.Interfaces
{
    public interface IForumRepository
    {
        Forum GetById(int id);
        public void Add(Forum forum);
        public void Delete(int id);
        public void Update(int id);
    }
}
