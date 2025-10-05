using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Tasks.Commands
{
    public class AddCommentCommand : IRequest<CommentResponse>
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;

        public AddCommentCommand(Guid taskId, Guid userId, string content)
        {
            TaskId = taskId;
            UserId = userId;
            Content = content;
        }
    }
}