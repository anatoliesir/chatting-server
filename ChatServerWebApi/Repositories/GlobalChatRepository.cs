using ChatServerWebApi.Models;
using ChatServerWebApi.Data;
using Microsoft.EntityFrameworkCore;
using ChatApp.Shared.Models;

namespace ChatServerWebApi.Repositories
{
    public class GlobalChatRepository : IGlobalChatRepository
    {
        private readonly AppDbContext _context;
        public GlobalChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GlobalChat>> GetAllAsync()
        {
            return await _context.GlobalChat.ToListAsync();
        }

        public async Task<GlobalChat?> GetByIdAsync(int id)
        {
            return await _context.GlobalChat.FindAsync(id);
        }

        public async Task AddAsync(GlobalChat message)
        {
            await _context.GlobalChat.AddAsync(message);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(GlobalChat newMessage)
        {
            var oldMessage = await _context.GlobalChat.FindAsync(newMessage.Id);
            if (oldMessage == null) return;

            _context.Entry(oldMessage).CurrentValues.SetValues(newMessage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _context.GlobalChat.FindAsync(id);
            if (message == null) return;

            _context.GlobalChat.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
