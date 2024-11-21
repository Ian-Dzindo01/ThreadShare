using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    // IMPLEMENT MANUAL CRUD METHODS FOR CACHE INVALIDATION
    public async Task<User> GetUserById(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userManager.Users.ToListAsync();
    }
}
