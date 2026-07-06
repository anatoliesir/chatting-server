using ChatApp.Application.Common.Interfaces;
using ChatApp.Shared.Models;
using ChatApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<List<User>> GetAllAsync()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        //public async Task<User?> GetByIdAsync(int id)
        //{
        //    return await _context.Users.FindAsync(id);
        //}

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateAsync(User newUser)
        //{
        //    var oldUser = await _context.Users.FindAsync(newUser.Id);
        //    if(oldUser == null)
        //    {
        //        return;
        //    }

        //    _context.Users.Entry(oldUser).CurrentValues.SetValues(newUser);
        //    await _context.SaveChangesAsync();
        //}

        public async Task DeleteAsync(int id)
        {
            var user =  await _context.Users.FindAsync(id);
            if (user == null) return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByUsernameAsync(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null) return null;
            return user;
        }
    }
}
