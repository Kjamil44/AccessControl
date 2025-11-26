using AccessControl.Contracts;
using MassTransit;

namespace AccessControl.Messaging.Consumers
{
    public class UnlockTriggerConsumer : IConsumer<TriggerUnlock>
    {
        public async Task Consume(ConsumeContext<TriggerUnlock> ctx)
        {
            // Call your controller/driver here
            var now = DateTime.Now.ToString();
            var isAllowed = ctx.Message.IsAllowed;

            if (isAllowed)
            {
                await ctx.Publish(new UnlockTriggerGranted(ctx.Message.LockId, DateTimeOffset.UtcNow, ctx.Message.CorrelationId));
                Console.WriteLine($"[EVENT] UnlockTriggerGranted  LockId={ctx.Message.LockId} At={now:o} Corr={ctx.Message.CorrelationId}");
            }
            else
            {
                await ctx.Publish(new UnlockDenied(ctx.Message.LockId, "Device NACK", DateTimeOffset.UtcNow, ctx.Message.CorrelationId));
                Console.WriteLine($"[EVENT] UnlockTriggerDenied  LockId={ctx.Message.LockId} Reason=\"{ctx.Message.Reason}\" At={now:o} Corr={ctx.Message.CorrelationId}");
            }
        }
    }
}