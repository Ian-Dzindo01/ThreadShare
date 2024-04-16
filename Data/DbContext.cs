using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ThreadShare.Models;

namespace PostgreSQL.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("DbConnectionString"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

    }
}