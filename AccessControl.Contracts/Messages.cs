namespace AccessControl.Contracts;

//TODO: The arguments that the records are having are open for change in order to comply with the API Signatures
public record UnlockDoor(Guid DoorId, Guid CorrelationId, string? RequestedBy = null);
public record LockDoor(Guid DoorId, Guid CorrelationId, string? RequestedBy = null);

public record DoorUnlocked(Guid DoorId, DateTimeOffset At, Guid CorrelationId);
public record AccessDenied(Guid DoorId, string Reason, DateTimeOffset At, Guid CorrelationId);
