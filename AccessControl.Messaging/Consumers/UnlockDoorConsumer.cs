using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using AccessControl.Contracts;

namespace AccessControl.Messaging.Consumers;

public class UnlockDoorConsumer : IConsumer<UnlockDoor>
{
    private readonly ILogger<UnlockDoorConsumer> _log;

    public UnlockDoorConsumer(ILogger<UnlockDoorConsumer> log) => _log = log;

    public async Task Consume(ConsumeContext<UnlockDoor> context)
    {
        var msg = context.Message;
        _log.LogInformation("UnlockDoor received for {DoorId} (corr:{Corr})", msg.DoorId, msg.CorrelationId);

        // TODO: integrate with your controller/protocol here
        var success = await DoorDriver.UnlockAsync(msg.DoorId);

        if (success)
        {
            await context.Publish(new DoorUnlocked(msg.DoorId, DateTimeOffset.UtcNow, msg.CorrelationId));
            _log.LogInformation("Door {DoorId} unlocked", msg.DoorId);
        }
        else
        {
            await context.Publish(new AccessDenied(msg.DoorId, "Device did not acknowledge unlock",
                                                   DateTimeOffset.UtcNow, msg.CorrelationId));
            _log.LogWarning("Door {DoorId} unlock denied", msg.DoorId);

            // Throw to trigger retry; after retries it lands in the endpoint's _skipped queue
            throw new InvalidOperationException("Unlock failed at device level");
        }
    }

    // minimal stub — replace with your real device integration
    static class DoorDriver
    {
        public static Task<bool> UnlockAsync(Guid doorId)
            => Task.FromResult(true);
    }
}
