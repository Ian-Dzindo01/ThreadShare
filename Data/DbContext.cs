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

            //Find a better way to do this

            //modelBuilder.Entity<Comment>()
            //    .HasOne(c => c.User)
            //    .WithMany() 
            //    .HasForeignKey(c => c.UserId)
            //    .IsRequired();  

            // If UserId is required in Comment entity
                   //modelBuilder.Entity<Post>()
            //    .HasOne(p => p.User)
            //    .WithMany(u => u.Posts)
            //    .HasForeignKey(p => p.UserId);

            // modelBuilder.Entity<Comment>()
            //     .HasOne(c => c.Post)
            //     .WithMany(p => p.Comments)
            //     .HasForeignKey(c => c.PostId);

            // modelBuilder.Entity<Post>()
            //     .HasOne(p => p.Forum)
            //     .WithMany(f => f.Posts)
            //     .HasForeignKey(p => p.ForumId);

            // modelBuilder.Entity<Forum>()
            //     .HasOne(f => f.User)
            //     .WithMany()
            //     .HasForeignKey(f => f.UserId);
        }
    }
}