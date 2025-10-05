using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<ProjectTask?> GetByIdAsync(Guid id);
        Task<List<ProjectTask>> GetProjectTasksAsync(Guid projectId);
        Task AddAsync(ProjectTask task);
        void Update(ProjectTask task);
        void Delete(ProjectTask task);
    }
}