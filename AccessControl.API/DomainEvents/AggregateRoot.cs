namespace AccessControl.API.DomainEvents
{
    public abstract class AggregateRoot
    {
        private readonly List<IDomainEvent> _events = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

        protected void Raise(IDomainEvent @event) => _events.Add(@event);
        public void ClearDomainEvents() => _events.Clear();
    }
}