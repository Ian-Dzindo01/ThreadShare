using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;
using ThreadShare.Service.Interfaces;

namespace ThreadShare.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }
    }
}
