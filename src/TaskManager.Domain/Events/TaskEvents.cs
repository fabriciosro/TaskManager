using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Events
{
    public class TaskCreatedEvent : BaseEvent
    {
        public Guid TaskId { get; }
        public string Title { get; }
        public Guid ProjectId { get; }

        public TaskCreatedEvent(Guid taskId, string title, Guid projectId)
        {
            TaskId = taskId;
            Title = title;
            ProjectId = projectId;
        }
    }

    public class TaskUpdatedEvent : BaseEvent
    {
        public Guid TaskId { get; }
        public string Field { get; }
        public string OldValue { get; }
        public string NewValue { get; }

        public TaskUpdatedEvent(Guid taskId, string field, string oldValue, string newValue)
        {
            TaskId = taskId;
            Field = field;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class TaskStatusChangedEvent : BaseEvent
    {
        public Guid TaskId { get; }
        public TaskStatus OldStatus { get; }
        public TaskStatus NewStatus { get; }
        public Guid ChangedByUserId { get; }

        public TaskStatusChangedEvent(Guid taskId, TaskStatus oldStatus, TaskStatus newStatus, Guid changedByUserId)
        {
            TaskId = taskId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            ChangedByUserId = changedByUserId;
        }
    }

    public class CommentAddedEvent : BaseEvent
    {
        public Guid TaskId { get; }
        public Guid CommentId { get; }
        public Guid UserId { get; }

        public CommentAddedEvent(Guid taskId, Guid commentId, Guid userId)
        {
            TaskId = taskId;
            CommentId = commentId;
            UserId = userId;
        }
    }
}