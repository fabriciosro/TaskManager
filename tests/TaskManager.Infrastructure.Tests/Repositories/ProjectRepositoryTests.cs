using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;
using Xunit;

namespace TaskManager.Infrastructure.Tests.Repositories
{
    public class ProjectRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ProjectRepository _repository;

        public ProjectRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new ProjectRepository(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProject_WhenExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project(projectId, "Test Project", "Description", Guid.NewGuid());
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(projectId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(projectId);
            result.Name.Should().Be("Test Project");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserProjectsAsync_ShouldReturnUserProjects()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userProject = new Project(Guid.NewGuid(), "User Project", "Description", userId);
            var otherProject = new Project(Guid.NewGuid(), "Other Project", "Description", Guid.NewGuid());

            await _context.Projects.AddRangeAsync(userProject, otherProject);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserProjectsAsync(userId);

            // Assert
            result.Should().ContainSingle();
            result.First().Name.Should().Be("User Project");
        }

        [Fact]
        public async Task AddAsync_ShouldAddProject()
        {
            // Arrange
            var project = new Project(Guid.NewGuid(), "New Project", "Description", Guid.NewGuid());

            // Act
            await _repository.AddAsync(project);
            await _context.SaveChangesAsync();

            // Assert
            var savedProject = await _context.Projects.FindAsync(project.Id);
            savedProject.Should().NotBeNull();
            savedProject.Name.Should().Be("New Project");
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}