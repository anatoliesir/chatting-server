using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Expression-bodied properties pointing to our database sets
        public DbSet<User> Users => Set<User>();
        public DbSet<GlobalChat> GlobalChat => Set<GlobalChat>(); 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configuration for the User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Fluent API configuration for the GlobalChat entity
            modelBuilder.Entity<GlobalChat>(entity =>
            {
                entity.HasKey(e => e.Id); // Configures primary key for chat table
            });
        }
    }
}