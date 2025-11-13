using AccessControl.Contracts;
using MassTransit;

namespace AccessControl.Messaging.Consumers
{
    public class UnlockTriggerConsumer : IConsumer<TriggerUnlock>
    {
        public async Task Consume(ConsumeContext<TriggerUnlock> ctx)
        {
            // Call your controller/driver here
            var isAllowed = ctx.Message.IsAllowed;

            if (isAllowed)
                await ctx.Publish(new UnlockTriggered(ctx.Message.LockId, DateTimeOffset.UtcNow, ctx.Message.CorrelationId));
            else
                await ctx.Publish(new UnlockDenied(ctx.Message.LockId, "Device NACK", DateTimeOffset.UtcNow, ctx.Message.CorrelationId));
        }
    }
}