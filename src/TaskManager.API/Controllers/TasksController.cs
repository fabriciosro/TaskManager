using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Queries;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskResponse>>> GetProjectTasks(Guid projectId)
        {
            try
            {
                var query = new GetProjectTasksQuery(projectId);
                var tasks = await _mediator.Send(query);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskResponse>> GetTask(Guid taskId)
        {
            try
            {
                var query = new GetTaskQuery(taskId);
                var task = await _mediator.Send(query);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponse>> CreateTask(
            Guid projectId,
            [FromHeader] Guid userId,
            [FromBody] CreateTaskRequest request)
        {
            try
            {
                var command = new CreateTaskCommand(projectId, userId, request.Title, request.Description, request.DueDate, request.Priority);
                var task = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetTask), new { projectId, taskId = task.Id }, task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult<TaskResponse>> UpdateTask(
            Guid taskId,
            [FromHeader] Guid userId,
            [FromBody] UpdateTaskRequest request)
        {
            try
            {
                var command = new UpdateTaskCommand(taskId, userId)
                {
                    Title = request.Title,
                    Description = request.Description,
                    DueDate = request.DueDate,
                    Status = request.Status
                };

                var task = await _mediator.Send(command);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(Guid taskId)
        {
            try
            {
                var command = new DeleteTaskCommand(taskId);
                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{taskId}/comments")]
        public async Task<ActionResult<CommentResponse>> AddComment(
            Guid taskId,
            [FromHeader] Guid userId,
            [FromBody] AddCommentRequest request)
        {
            try
            {
                var command = new AddCommentCommand(taskId, userId, request.Content);
                var comment = await _mediator.Send(command);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}