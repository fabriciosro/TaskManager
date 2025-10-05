using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Application.Reports.Queries
{
    public class GetPerformanceReportQueryHandler : IRequestHandler<GetPerformanceReportQuery, PerformanceReportResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPerformanceReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PerformanceReportResponse> Handle(GetPerformanceReportQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var allProjects = new List<Domain.Entities.Project>();

            foreach (var user in users)
            {
                var userProjects = await _unitOfWork.Projects.GetUserProjectsAsync(user.Id);
                allProjects.AddRange(userProjects);
            }

            var completedTasks = allProjects
                .SelectMany(p => p.Tasks)
                .Where(t => t.Status == TaskStatus.Completed &&
                           t.CreatedAt >= request.StartDate &&
                           t.CreatedAt <= request.EndDate)
                .ToList();

            var userTaskCounts = users.Select(user =>
                allProjects
                    .Where(p => p.UserId == user.Id)
                    .SelectMany(p => p.Tasks)
                    .Count(t => t.Status == TaskStatus.Completed &&
                               t.CreatedAt >= request.StartDate &&
                               t.CreatedAt <= request.EndDate))
                .Where(count => count > 0)
                .ToList();

            var averageCompletedTasks = userTaskCounts.Any() ? userTaskCounts.Average() : 0;

            return new PerformanceReportResponse
            {
                AverageCompletedTasks = Math.Round(averageCompletedTasks, 2),
                TotalCompletedTasks = completedTasks.Count,
                TotalUsers = users.Count,
                PeriodStart = request.StartDate,
                PeriodEnd = request.EndDate
            };
        }
    }
}