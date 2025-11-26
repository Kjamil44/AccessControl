namespace AccessControl.API.DomainEvents
{
    public record LockTriggeredDomainEvent(Guid LockId, string CardNumber, bool IsAllowed, string Reason = "") : IDomainEvent;

    public record UnlockTriggeredDomainEvent(Guid LockId, string CardNumber, bool IsAllowed , string Reason = "") : IDomainEvent;

}
