using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Projects.Commands;
using TaskManager.Application.Projects.Queries;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectResponse>>> GetUserProjects([FromHeader] Guid userId)
        {
            try
            {
                var query = new GetUserProjectsQuery(userId);
                var projects = await _mediator.Send(query);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectResponse>> GetProject(Guid projectId)
        {
            try
            {
                var query = new GetProjectQuery(projectId);
                var project = await _mediator.Send(query);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> CreateProject(
            [FromHeader] Guid userId,
            [FromBody] CreateProjectRequest request)
        {
            try
            {
                var command = new CreateProjectCommand(userId, request.Name, request.Description);
                var project = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetProject), new { projectId = project.Id }, project);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject([FromHeader] Guid userId, Guid projectId)
        {
            try
            {
                var command = new DeleteProjectCommand(projectId, userId);
                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}