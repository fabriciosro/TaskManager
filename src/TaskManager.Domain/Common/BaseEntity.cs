using TaskManager.Domain.Events;

namespace TaskManager.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        // Mude para transient - não deve ser persistido
        private List<BaseEvent> _domainEvents;
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents ??= new List<BaseEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id == default || other.Id == default)
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}