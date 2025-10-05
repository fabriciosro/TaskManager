using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
    }

    public class UpdateTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Domain.Enums.TaskStatus? Status { get; set; }
    }

    public class TaskResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TaskHistoryResponse> History { get; set; } = new();
        public List<CommentResponse> Comments { get; set; } = new();
    }

    public class TaskHistoryResponse
    {
        public string FieldChanged { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public Guid ChangedByUserId { get; set; }
    }

    public class CommentResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
    }

    public class AddCommentRequest
    {
        public string Content { get; set; } = string.Empty;
    }
}