using ChatServerWebApi.Models;
using ChatApp.Shared.Models;

namespace ChatServerWebApi.Repositories
{
    public interface IGlobalChatRepository
    {
        public Task<List<GlobalChat>> GetAllAsync();
        public Task<GlobalChat?> GetByIdAsync(int id);
        public Task AddAsync(GlobalChat message);
        public Task UpdateAsync(GlobalChat newMessage);
        public Task DeleteAsync(int id);
    }
}
