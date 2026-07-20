using ChatApp.Domain.Entities;
namespace ChatApp.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        //public Task<List<User>> GetAllAsync();
        //public Task<User?> GetByIdAsync(int id);
        public Task AddAsync(User user);
        //public Task UpdateAsync(User newUser);
        public Task DeleteAsync(int id);

        public Task<User?> GetByUsernameAsync(string userName);
    }
}
