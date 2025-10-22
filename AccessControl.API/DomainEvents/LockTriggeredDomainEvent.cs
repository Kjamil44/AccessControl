namespace AccessControl.API.DomainEvents
{
    public record LockTriggeredDomainEvent(Guid LockId, int CardNumber) : IDomainEvent;
}
