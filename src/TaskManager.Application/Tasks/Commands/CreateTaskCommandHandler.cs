using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tasks.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
            if (project == null)
                throw new DomainException("Project not found");

            var task = project.AddTask(
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority
            );

            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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