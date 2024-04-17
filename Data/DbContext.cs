using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThreadShare.Models;

namespace ThreadShare.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
            
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}