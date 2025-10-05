using MediatR;

namespace TaskManager.Application.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public Guid TaskId { get; set; }

        public DeleteTaskCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}