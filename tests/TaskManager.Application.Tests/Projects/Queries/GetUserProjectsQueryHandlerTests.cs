using FluentAssertions;
using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Projects.Queries;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Application.Tests.Projects.Queries
{
    public class GetUserProjectsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetUserProjectsQueryHandler _handler;

        public GetUserProjectsQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new GetUserProjectsQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserProjects()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserProjectsQuery(userId);

            var projects = new List<Project>
            {
                new Project(Guid.NewGuid(), "Project 1", "Description 1", userId),
                new Project(Guid.NewGuid(), "Project 2", "Description 2", userId)
            };

            _unitOfWorkMock.Setup(u => u.Projects.GetUserProjectsAsync(userId)).ReturnsAsync(projects);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Project 1");
            result[1].Name.Should().Be("Project 2");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenNoProjects()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserProjectsQuery(userId);

            _unitOfWorkMock.Setup(u => u.Projects.GetUserProjectsAsync(userId)).ReturnsAsync(new List<Project>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }
    }
}