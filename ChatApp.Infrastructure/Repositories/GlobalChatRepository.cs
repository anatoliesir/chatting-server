using Microsoft.EntityFrameworkCore;
using ChatApp.Domain.Entities;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Infrastructure.Persistence;

namespace ChatApp.Infrastructure.Repositories
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
        public async Task DeleteAllAsync()
        {
            await _context.GlobalChat.ExecuteDeleteAsync();
        }

        public async Task TrimMessagesAsync(int maxLimit)
        {
            var totalMessages = await _context.GlobalChat.CountAsync();

            if (totalMessages > maxLimit)
            {
                int excessCount = totalMessages - maxLimit;

                // Delete the oldest messages, by selecting them by the time (SentAt)
                var oldestMessages = await _context.GlobalChat
                    .OrderBy(m => m.SentAt)
                    .Take(excessCount)
                    .ToListAsync();

                _context.GlobalChat.RemoveRange(oldestMessages);
                await _context.SaveChangesAsync();
            }
        }
    }
}
