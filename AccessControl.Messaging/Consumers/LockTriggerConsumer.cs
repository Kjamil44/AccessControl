using AccessControl.Contracts;
using MassTransit;

namespace AccessControl.Messaging.Consumers
{
    public class LockTriggerConsumer : IConsumer<TriggerLock>
    {
        public async Task Consume(ConsumeContext<TriggerLock> ctx)
        {
            // Call your controller/driver here
            var ok = true;

            if (ok)
                await ctx.Publish(new LockTriggered(ctx.Message.LockId, DateTimeOffset.UtcNow, ctx.Message.CorrelationId));
            else
                await ctx.Publish(new LockDenied(ctx.Message.LockId, "Device NACK", DateTimeOffset.UtcNow, ctx.Message.CorrelationId));
        }
    }
}