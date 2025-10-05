using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task<bool> ExistsAsync(Guid id);
    }
}