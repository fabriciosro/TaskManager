using MediatR;
using TaskManager.Application.DTOs;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Application.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest<TaskResponse>
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus? Status { get; set; }

        public UpdateTaskCommand(Guid taskId, Guid userId)
        {
            TaskId = taskId;
            UserId = userId;
        }
    }
}