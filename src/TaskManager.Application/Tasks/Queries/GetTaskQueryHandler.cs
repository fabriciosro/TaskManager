using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tasks.Queries
{
    public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTaskQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskResponse> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);

            if (task == null)
                throw new DomainException("Task not found");

            return MapToTaskResponse(task);
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