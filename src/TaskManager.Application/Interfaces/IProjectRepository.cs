using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task<List<Project>> GetUserProjectsAsync(Guid userId);
        Task AddAsync(Project project);
        void Update(Project project);
        void Delete(Project project);
        Task<bool> ExistsAsync(Guid id);
    }
}