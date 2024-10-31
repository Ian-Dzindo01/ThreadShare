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
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // Cascade delete should happen between Post and Comments and Forum and Posts.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fix IdentityUserLogin<string>
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Forum)
                .WithMany(f => f.Posts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(f => f.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .Property(p => p.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Forum>()
                .Property(f => f.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("NOW()");
        }
    }
}