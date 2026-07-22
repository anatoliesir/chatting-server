using ChatApp.Domain.Entities;

namespace ChatApp.Application.Common.Interfaces
{
    public interface IGlobalChatRepository
    {
        public Task<List<GlobalChat>> GetAllAsync();
        public Task<GlobalChat?> GetByIdAsync(int id);
        public Task AddAsync(GlobalChat message);
        public Task UpdateAsync(GlobalChat newMessage);
        public Task DeleteAsync(int id);
        public Task DeleteAllAsync();
        public Task TrimMessagesAsync(int maxLimit);
    }
}
