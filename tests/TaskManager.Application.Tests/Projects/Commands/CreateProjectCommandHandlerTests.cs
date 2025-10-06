using FluentAssertions;
using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Projects.Commands;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Application.Tests.Projects.Commands
{
    public class CreateProjectCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateProjectCommandHandler _handler;

        public CreateProjectCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateProjectCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProject_WhenValidCommand()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateProjectCommand(userId, "Test Project", "Test Description");

            _unitOfWorkMock.Setup(u => u.Projects.AddAsync(It.IsAny<Project>()));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Project");
            result.Description.Should().Be("Test Description");
            result.TaskCount.Should().Be(0);

            _unitOfWorkMock.Verify(u => u.Projects.AddAsync(It.Is<Project>(p =>
                p.Name == "Test Project" && p.UserId == userId)), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}