using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Reports.Queries
{
    public class GetPerformanceReportQuery : IRequest<PerformanceReportResponse>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public GetPerformanceReportQuery(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}