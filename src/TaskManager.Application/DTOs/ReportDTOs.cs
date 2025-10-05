namespace TaskManager.Application.DTOs
{
    public class PerformanceReportResponse
    {
        public double AverageCompletedTasks { get; set; }
        public int TotalCompletedTasks { get; set; }
        public int TotalUsers { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}