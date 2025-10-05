using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Reports.Queries;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("performance")]
        public async Task<ActionResult<PerformanceReportResponse>> GetPerformanceReport(
            [FromHeader] Guid userId,
            [FromHeader] UserRole userRole,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                // Check if user has manager role
                if (userRole != UserRole.Manager && userRole != UserRole.Admin)
                    return Forbid();

                var actualStartDate = startDate ?? DateTime.UtcNow.AddDays(-30);
                var actualEndDate = endDate ?? DateTime.UtcNow;

                var query = new GetPerformanceReportQuery(actualStartDate, actualEndDate);
                var report = await _mediator.Send(query);

                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}