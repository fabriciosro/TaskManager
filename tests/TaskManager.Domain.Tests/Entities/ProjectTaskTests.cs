using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Events;
using Xunit;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Tests.Entities
{
    public class ProjectTaskTests
    {
        [Fact]
        public void Constructor_ShouldCreateTask_WithValidParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var title = "Test Task";
            var description = "Test Description";
            var dueDate = DateTime.UtcNow.AddDays(7);
            var priority = TaskPriority.High;
            var projectId = Guid.NewGuid();

            // Act
            var task = new ProjectTask(id, title, description, dueDate, priority, projectId);

            // Assert
            task.Id.Should().Be(id);
            task.Title.Should().Be(title);
            task.Description.Should().Be(description);
            task.DueDate.Should().Be(dueDate);
            task.Priority.Should().Be(priority);
            task.Status.Should().Be(TaskStatus.Pending);
            task.ProjectId.Should().Be(projectId);
            task.DomainEvents.Should().ContainSingle(e => e is TaskCreatedEvent);
        }

        [Fact]
        public void UpdateTitle_ShouldUpdateTitle_AndAddHistory()
        {
            // Arrange
            var task = new ProjectTask(Guid.NewGuid(), "Old Title", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Medium, Guid.NewGuid());
            task.ClearDomainEvents();

            // Act
            task.UpdateTitle("New Title");

            // Assert
            task.Title.Should().Be("New Title");
            task.History.Should().ContainSingle(h => h.FieldChanged == "Title");
            task.DomainEvents.Should().ContainSingle(e => e is TaskUpdatedEvent);
        }

        [Fact]
        public void UpdateTitle_ShouldThrow_WhenTitleIsEmpty()
        {
            // Arrange
            var task = new ProjectTask(Guid.NewGuid(), "Valid Title", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Medium, Guid.NewGuid());

            // Act & Assert
            Assert.Throws<DomainException>(() => task.UpdateTitle(""));
        }

        [Fact]
        public void ChangeStatus_ShouldUpdateStatus_AndAddHistory()
        {
            // Arrange
            var task = new ProjectTask(Guid.NewGuid(), "Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Medium, Guid.NewGuid());
            var userId = Guid.NewGuid();
            task.ClearDomainEvents();

            // Act
            task.ChangeStatus(TaskStatus.InProgress, userId);

            // Assert
            task.Status.Should().Be(TaskStatus.InProgress);
            task.History.Should().ContainSingle(h => h.FieldChanged == "Status");
            task.DomainEvents.Should().ContainSingle(e => e is TaskStatusChangedEvent);
        }

        [Fact]
        public void AddComment_ShouldAddComment_AndAddHistory()
        {
            // Arrange
            var task = new ProjectTask(Guid.NewGuid(), "Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Medium, Guid.NewGuid());
            var userId = Guid.NewGuid();
            task.ClearDomainEvents();

            // Act
            var comment = task.AddComment("Test comment", userId);

            // Assert
            task.Comments.Should().ContainSingle();
            comment.Content.Should().Be("Test comment");
            comment.UserId.Should().Be(userId);
            task.History.Should().ContainSingle(h => h.FieldChanged == "Comment");
            task.DomainEvents.Should().ContainSingle(e => e is CommentAddedEvent);
        }

        [Fact]
        public void AddComment_ShouldThrow_WhenContentIsEmpty()
        {
            // Arrange
            var task = new ProjectTask(Guid.NewGuid(), "Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Medium, Guid.NewGuid());
            var userId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException>(() => task.AddComment("", userId));
        }
    }
}