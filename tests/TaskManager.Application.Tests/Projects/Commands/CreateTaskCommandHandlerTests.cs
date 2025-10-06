using FluentAssertions;
using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using Xunit;

namespace TaskManager.Application.Tests.Tasks.Commands
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateTaskCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateTask_WhenValidCommand()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new CreateTaskCommand(
                projectId, userId, "Test Task", "Description",
                DateTime.UtcNow.AddDays(7), TaskPriority.High);

            var project = new Project(projectId, "Test Project", "Description", userId);
            _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync(project);
            _unitOfWorkMock.Setup(u => u.Tasks.AddAsync(It.IsAny<ProjectTask>()));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Task");
            result.Priority.Should().Be(TaskPriority.High);

            _unitOfWorkMock.Verify(u => u.Tasks.AddAsync(It.Is<ProjectTask>(t =>
                t.Title == "Test Task" && t.ProjectId == projectId)), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenProjectNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new CreateTaskCommand(
                projectId, userId, "Test Task", "Description",
                DateTime.UtcNow.AddDays(7), TaskPriority.High);

            _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}