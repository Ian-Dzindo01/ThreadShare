using ThreadShare.Models;

namespace ThreadShare.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(string userId);
        public Task<IEnumerable<User>> GetAllUsers();
    }
}
