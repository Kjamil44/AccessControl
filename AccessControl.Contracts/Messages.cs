namespace AccessControl.Contracts;

public record TriggerLock(Guid LockId, string CardNumber, Guid CorrelationId, bool IsAllowed, string Reason);
public record LockTriggerGranted(Guid LockId, DateTimeOffset At, Guid CorrelationId);
public record LockTriggerDenied(Guid LockId, string Reason, DateTimeOffset At, Guid CorrelationId);

public record TriggerUnlock(Guid LockId, string CardNumber, Guid CorrelationId, bool IsAllowed, string Reason);
public record UnlockTriggerGranted(Guid LockId, DateTimeOffset At, Guid CorrelationId);
public record UnlockDenied(Guid LockId, string Reason, DateTimeOffset At, Guid CorrelationId);