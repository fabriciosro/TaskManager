using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Tasks.Queries
{
    public class GetTaskQuery : IRequest<TaskResponse>
    {
        public Guid TaskId { get; set; }

        public GetTaskQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}