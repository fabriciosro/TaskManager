using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Projects.Commands
{
    public class CreateProjectCommand : IRequest<ProjectResponse>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public CreateProjectCommand(Guid userId, string name, string description)
        {
            UserId = userId;
            Name = name;
            Description = description;
        }
    }
}