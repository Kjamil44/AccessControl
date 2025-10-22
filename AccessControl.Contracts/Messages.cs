namespace AccessControl.Contracts;

//TODO: The arguments that the records are having are open for change in order to comply with the API Signatures
public record TriggerLock(Guid LockId, int CardNumber, Guid CorrelationId);
public record LockTriggered(Guid LockId, DateTimeOffset At, Guid CorrelationId);
public record LockDenied(Guid LockId, string Reason, DateTimeOffset At, Guid CorrelationId);
