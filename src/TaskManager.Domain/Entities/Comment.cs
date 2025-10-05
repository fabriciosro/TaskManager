using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Common;
using TaskManager.Domain.Events;

namespace TaskManager.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid UserId { get; private set; }
        public Guid TaskId { get; private set; }
        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => base.DomainEvents;

        private Comment() { } 

        public Comment(Guid id, string content, Guid userId, Guid taskId)
        {
            Id = id;
            Content = content;
            UserId = userId;
            TaskId = taskId;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string content)
        {
            Content = content;
        }
    }
}   