using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<TaskResponse>
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }

        public CreateTaskCommand(Guid projectId, Guid userId, string title, string description, DateTime dueDate, TaskPriority priority)
        {
            ProjectId = projectId;
            UserId = userId;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
        }
    }
}