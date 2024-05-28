using ThreadShare.Models;

namespace ThreadShare.Service.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserById(string userId);
        public Task<IEnumerable<User>> GetAllUsers();
    }
}
