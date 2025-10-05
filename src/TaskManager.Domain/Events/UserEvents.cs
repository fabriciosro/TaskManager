using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Events
{
    public class UserCreatedEvent : BaseEvent
    {
        public Guid UserId { get; }
        public string Name { get; }
        public string Email { get; }

        public UserCreatedEvent(Guid userId, string name, string email)
        {
            UserId = userId;
            Name = name;
            Email = email;
        }
    }

    public class UserProfileUpdatedEvent : BaseEvent
    {
        public Guid UserId { get; }
        public string NewName { get; }
        public string NewEmail { get; }

        public UserProfileUpdatedEvent(Guid userId, string newName, string newEmail)
        {
            UserId = userId;
            NewName = newName;
            NewEmail = newEmail;
        }
    }

    public class UserRoleChangedEvent : BaseEvent
    {
        public Guid UserId { get; }
        public UserRole NewRole { get; }

        public UserRoleChangedEvent(Guid userId, UserRole newRole)
        {
            UserId = userId;
            NewRole = newRole;
        }
    }
}