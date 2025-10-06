using FluentAssertions;
using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Projects.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using Xunit;

namespace TaskManager.Application.Tests.Projects.Commands
{
    public class DeleteProjectCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteProjectCommandHandler _handler;

        public DeleteProjectCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteProjectCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteProject_WhenNoPendingTasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new DeleteProjectCommand(projectId, userId);

            var project = new Project(projectId, "Test Project", "Description", userId);
            _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync(project);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.Projects.Delete(project), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenProjectHasPendingTasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new DeleteProjectCommand(projectId, userId);

            var project = new Project(projectId, "Test Project", "Description", userId);
            project.AddTask("Pending Task", "Description", DateTime.UtcNow.AddDays(1), Domain.Enums.TaskPriority.Medium);

            _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync(project);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProjectNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new DeleteProjectCommand(projectId, userId);

            _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}