using ChatServerWebApi.Models;
using Microsoft.EntityFrameworkCore;
using ChatApp.Shared.Models;

namespace ChatServerWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GlobalChat> GlobalChat { get; set; }
    }
}
