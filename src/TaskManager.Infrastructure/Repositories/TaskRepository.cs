using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.History)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<ProjectTask>> GetProjectTasksAsync(Guid projectId)
        {
            return await _context.Tasks
                .Include(t => t.History)
                .Include(t => t.Comments)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddAsync(ProjectTask task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public void Update(ProjectTask task)
        {
            _context.Tasks.Update(task);
        }

        public void Delete(ProjectTask task)
        {
            _context.Tasks.Remove(task);
        }
    }
}