namespace AccessControl.API.DomainEvents
{
    public record LockTriggeredDomainEvent(Guid LockId, string CardNumber) : IDomainEvent;
}
