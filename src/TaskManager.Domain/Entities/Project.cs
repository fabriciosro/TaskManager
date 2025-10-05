using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Common;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Events;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.ValueObjects;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid UserId { get; private set; }

        private readonly List<ProjectTask> _tasks = new();
        public IReadOnlyCollection<ProjectTask> Tasks => _tasks.AsReadOnly();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => base.DomainEvents;

        private Project() {}

        public Project(Guid id, string name, string description, Guid userId)
        {
            Id = id;
            Name = name;
            Description = description;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;

            ValidateState();
            AddDomainEvent(new ProjectCreatedEvent(Id, Name, UserId));
        }

        public void Update(string name, string description)
        {
            Name = name;
            Description = description;

            ValidateState();
            AddDomainEvent(new ProjectUpdatedEvent(Id, Name));
        }

        public ProjectTask AddTask(string title, string description, DateTime dueDate, TaskPriority priority)
        {
            if (_tasks.Count >= ProjectLimits.MAX_TASKS_PER_PROJECT)
            {
                throw new DomainException($"Project cannot have more than {ProjectLimits.MAX_TASKS_PER_PROJECT} tasks");
            }

            var task = new ProjectTask(Guid.NewGuid(), title, description, dueDate, priority, Id);
            _tasks.Add(task);

            AddDomainEvent(new TaskAddedToProjectEvent(Id, task.Id, title));

            return task;
        }

        public void RemoveTask(ProjectTask task)
        {
            if (!_tasks.Contains(task))
            {
                throw new DomainException("Task not found in project");
            }

            _tasks.Remove(task);
            AddDomainEvent(new TaskRemovedFromProjectEvent(Id, task.Id));
        }

        public bool HasPendingTasks()
        {
            return _tasks.Any(t => t.Status == TaskStatus.Pending);
        }

        public int GetCompletedTasksCount()
        {
            return _tasks.Count(t => t.Status == TaskStatus.Completed);
        }

        private void ValidateState()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new DomainException("Project name is required");

            if (Name.Length > 100)
                throw new DomainException("Project name cannot exceed 100 characters");

            if (Description?.Length > 500)
                throw new DomainException("Project description cannot exceed 500 characters");
        }
    }
}