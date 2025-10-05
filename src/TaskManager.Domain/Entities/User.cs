using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Common;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Events;

namespace TaskManager.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navigation properties
        private readonly List<Project> _projects = new();
        public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

        [NotMapped] 
        public IReadOnlyCollection<BaseEvent> DomainEvents => base.DomainEvents;

        private User() { } // For EF Core

        public User(Guid id, string name, string email, UserRole role = UserRole.User)
        {
            Id = id;
            Name = name;
            Email = email;
            Role = role;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserCreatedEvent(Id, Name, Email));
        }

        public void UpdateProfile(string name, string email)
        {
            Name = name;
            Email = email;

            AddDomainEvent(new UserProfileUpdatedEvent(Id, Name, Email));
        }

        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
            AddDomainEvent(new UserRoleChangedEvent(Id, newRole));
        }
    }
}