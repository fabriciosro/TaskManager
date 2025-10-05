﻿using MediatR;

namespace TaskManager.Application.Projects.Commands
{
    public class DeleteProjectCommand : IRequest<bool>
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }

        public DeleteProjectCommand(Guid projectId, Guid userId)
        {
            ProjectId = projectId;
            UserId = userId;
        }
    }
}