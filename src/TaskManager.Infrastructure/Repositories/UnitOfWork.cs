using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IProjectRepository _projects;
        private ITaskRepository _tasks;
        private IUserRepository _users;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProjectRepository Projects => _projects ??= new ProjectRepository(_context);
        public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}