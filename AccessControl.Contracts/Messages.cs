namespace AccessControl.Contracts;

public record TriggerLock(Guid LockId, string CardNumber, Guid CorrelationId, bool IsAllowed);
public record LockTriggered(Guid LockId, DateTimeOffset At, Guid CorrelationId);
public record LockDenied(Guid LockId, string Reason, DateTimeOffset At, Guid CorrelationId);

public record TriggerUnlock(Guid LockId, string CardNumber, Guid CorrelationId);
public record UnlockTriggered(Guid LockId, DateTimeOffset At, Guid CorrelationId);
public record UnlockDenied(Guid LockId, string Reason, DateTimeOffset At, Guid CorrelationId);