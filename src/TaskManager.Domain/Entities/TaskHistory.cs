using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Common;
using TaskManager.Domain.Events;

namespace TaskManager.Domain.Entities
{
    public class TaskHistory : BaseEntity
    {
        public string FieldChanged { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }
        public DateTime ChangedAt { get; private set; }
        public Guid ChangedByUserId { get; private set; }
        public Guid TaskId { get; private set; }

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => base.DomainEvents;

        private TaskHistory() { } // For EF Core

        public TaskHistory(Guid id, string fieldChanged, string oldValue, string newValue, Guid changedByUserId, Guid taskId)
        {
            Id = id;
            FieldChanged = fieldChanged;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedByUserId = changedByUserId;
            TaskId = taskId;
            ChangedAt = DateTime.UtcNow;
        }
    }
}