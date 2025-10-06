using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Events;
using Xunit;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Tests.Entities
{
    public class ProjectTests
    {
        [Fact]
        public void Constructor_ShouldCreateProject_WithValidParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Test Project";
            var description = "Test Description";
            var userId = Guid.NewGuid();

            // Act
            var project = new Project(id, name, description, userId);

            // Assert
            project.Id.Should().Be(id);
            project.Name.Should().Be(name);
            project.Description.Should().Be(description);
            project.UserId.Should().Be(userId);
            project.Tasks.Should().BeEmpty();
            project.DomainEvents.Should().ContainSingle(e => e is ProjectCreatedEvent);
        }

        [Fact]
        public void AddTask_ShouldAddTask_WhenUnderLimit()
        {
            // Arrange
            var project = new Project(Guid.NewGuid(), "Test Project", "Description", Guid.NewGuid());

            // Act
            var task = project.AddTask("Test Task", "Task Description", DateTime.UtcNow.AddDays(7), TaskPriority.Medium);

            // Assert
            project.Tasks.Should().ContainSingle();
            task.Title.Should().Be("Test Task");
            task.ProjectId.Should().Be(project.Id);
            project.DomainEvents.Should().Contain(e => e is TaskAddedToProjectEvent);
        }

        [Fact]
        public void AddTask_ShouldThrow_WhenExceedingTaskLimit()
        {
            // Arrange
            var project = new Project(Guid.NewGuid(), "Test Project", "Description", Guid.NewGuid());

            // Add maximum tasks
            for (int i = 0; i < 20; i++)
            {
                project.AddTask($"Task {i}", $"Description {i}", DateTime.UtcNow.AddDays(1), TaskPriority.Low);
            }

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                project.AddTask("Extra Task", "Should Fail", DateTime.UtcNow.AddDays(1), TaskPriority.Medium));
        }

        [Fact]
        public void HasPendingTasks_ShouldReturnTrue_WhenPendingTasksExist()
        {
            // Arrange
            var project = new Project(Guid.NewGuid(), "Test Project", "Description", Guid.NewGuid());
            project.AddTask("Pending Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Medium);

            // Act
            var hasPending = project.HasPendingTasks();

            // Assert
            hasPending.Should().BeTrue();
        }

        [Fact]
        public void RemoveTask_ShouldRemoveTask()
        {
            // Arrange
            var project = new Project(Guid.NewGuid(), "Test Project", "Description", Guid.NewGuid());
            var task = project.AddTask("Test Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Medium);
            project.ClearDomainEvents();

            // Act
            project.RemoveTask(task);

            // Assert
            project.Tasks.Should().BeEmpty();
            project.DomainEvents.Should().ContainSingle(e => e is TaskRemovedFromProjectEvent);
        }

        [Fact]
        public void GetCompletedTasksCount_ShouldReturnCorrectCount()
        {
            // Arrange
            var project = new Project(Guid.NewGuid(), "Test Project", "Description", Guid.NewGuid());
            var userId = Guid.NewGuid();

            var task1 = project.AddTask("Task 1", "Desc", DateTime.UtcNow.AddDays(1), TaskPriority.Medium);
            var task2 = project.AddTask("Task 2", "Desc", DateTime.UtcNow.AddDays(1), TaskPriority.Medium);

            task1.ChangeStatus(TaskStatus.Completed, userId);

            // Act
            var completedCount = project.GetCompletedTasksCount();

            // Assert
            completedCount.Should().Be(1);
        }
    }
}