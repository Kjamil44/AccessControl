namespace AccessControl.API.DomainEvents
{
    public record LockTriggeredDomainEvent(Guid LockId, string CardNumber, bool IsAllowed) : IDomainEvent;

    public record UnlockTriggeredDomainEvent(Guid LockId, string CardNumber) : IDomainEvent;

}
