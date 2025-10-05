using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Projects.Queries;

namespace TaskManager.Application.Tasks.Queries
{
    public class GetProjectTasksQueryHandler : IRequestHandler<GetProjectTasksQuery, List<TaskResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProjectTasksQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TaskResponse>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _unitOfWork.Tasks.GetProjectTasksAsync(request.ProjectId);

            return tasks.Select(MapToTaskResponse).ToList();
        }

        private TaskResponse MapToTaskResponse(Domain.Entities.ProjectTask task)
        {
            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                History = task.History.Select(h => new TaskHistoryResponse
                {
                    FieldChanged = h.FieldChanged,
                    OldValue = h.OldValue,
                    NewValue = h.NewValue,
                    ChangedAt = h.ChangedAt,
                    ChangedByUserId = h.ChangedByUserId
                }).ToList(),
                Comments = task.Comments.Select(c => new CommentResponse
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId
                }).ToList()
            };
        }
    }
}