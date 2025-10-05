namespace TaskManager.Domain.Events
{
    public class ProjectCreatedEvent : BaseEvent
    {
        public Guid ProjectId { get; }
        public string Name { get; }
        public Guid UserId { get; }

        public ProjectCreatedEvent(Guid projectId, string name, Guid userId)
        {
            ProjectId = projectId;
            Name = name;
            UserId = userId;
        }
    }

    public class ProjectUpdatedEvent : BaseEvent
    {
        public Guid ProjectId { get; }
        public string NewName { get; }

        public ProjectUpdatedEvent(Guid projectId, string newName)
        {
            ProjectId = projectId;
            NewName = newName;
        }
    }

    public class TaskAddedToProjectEvent : BaseEvent
    {
        public Guid ProjectId { get; }
        public Guid TaskId { get; }
        public string TaskTitle { get; }

        public TaskAddedToProjectEvent(Guid projectId, Guid taskId, string taskTitle)
        {
            ProjectId = projectId;
            TaskId = taskId;
            TaskTitle = taskTitle;
        }
    }

    public class TaskRemovedFromProjectEvent : BaseEvent
    {
        public Guid ProjectId { get; }
        public Guid TaskId { get; }

        public TaskRemovedFromProjectEvent(Guid projectId, Guid taskId)
        {
            ProjectId = projectId;
            TaskId = taskId;
        }
    }
}