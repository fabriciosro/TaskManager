using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Common;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Events;
using TaskManager.Domain.Exceptions;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Entities
{
    public class ProjectTask : BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public Enums.TaskStatus Status { get; private set; }
        public TaskPriority Priority { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid ProjectId { get; private set; }

        // Navigation properties
        private readonly List<TaskHistory> _history = new();
        public IReadOnlyCollection<TaskHistory> History => _history.AsReadOnly();

        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => base.DomainEvents;

        private ProjectTask() { } // For EF Core

        public ProjectTask(Guid id, string title, string description, DateTime dueDate, TaskPriority priority, Guid projectId)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = TaskStatus.Pending;
            ProjectId = projectId;
            CreatedAt = DateTime.UtcNow;

            ValidateState();
            AddDomainEvent(new TaskCreatedEvent(Id, Title, ProjectId));
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Task title cannot be empty");

            var oldTitle = Title;
            Title = title;

            AddHistory("Title", oldTitle, title);
            AddDomainEvent(new TaskUpdatedEvent(Id, "Title", oldTitle, title));
        }

        public void UpdateDescription(string description)
        {
            var oldDescription = Description;
            Description = description;

            AddHistory("Description", oldDescription, description);
            AddDomainEvent(new TaskUpdatedEvent(Id, "Description", oldDescription, description));
        }

        public void UpdateDueDate(DateTime dueDate)
        {
            var oldDueDate = DueDate;
            DueDate = dueDate;

            AddHistory("DueDate", oldDueDate.ToString(), dueDate.ToString());
            AddDomainEvent(new TaskUpdatedEvent(Id, "DueDate", oldDueDate.ToString(), dueDate.ToString()));
        }

        public void ChangeStatus(Enums.TaskStatus newStatus, Guid changedByUserId)
        {
            var oldStatus = Status;
            Status = newStatus;

            AddHistory("Status", oldStatus.ToString(), newStatus.ToString(), changedByUserId);
            AddDomainEvent(new TaskStatusChangedEvent(Id, oldStatus, newStatus, changedByUserId));
        }

        public Comment AddComment(string content, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new DomainException("Comment content cannot be empty");

            var comment = new Comment(Guid.NewGuid(), content, userId, Id);
            _comments.Add(comment);

            AddHistory("Comment", string.Empty, $"Added comment: {content}", userId);
            AddDomainEvent(new CommentAddedEvent(Id, comment.Id, userId));

            return comment;
        }

        private void AddHistory(string field, string oldValue, string newValue, Guid? changedByUserId = null)
        {
            var history = new TaskHistory(
                Guid.NewGuid(),
                field,
                oldValue,
                newValue,
                changedByUserId ?? Guid.Empty, // In real scenario, this would be required
                Id
            );
            _history.Add(history);
        }

        private void ValidateState()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new DomainException("Task title is required");

            if (Title.Length > 100)
                throw new DomainException("Task title cannot exceed 100 characters");

            if (Description?.Length > 1000)
                throw new DomainException("Task description cannot exceed 1000 characters");
        }
    }
}