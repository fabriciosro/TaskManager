using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Projects.Commands
{
    public class UpdateProjectCommand : IRequest<ProjectResponse>
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public UpdateProjectCommand(Guid projectId, Guid userId, string name, string description)
        {
            ProjectId = projectId;
            UserId = userId;
            Name = name;
            Description = description;
        }
    }
}