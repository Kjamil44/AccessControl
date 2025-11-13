using AccessControl.Contracts;
using MassTransit;

namespace AccessControl.Messaging.Consumers
{
    public class LockTriggerConsumer : IConsumer<TriggerLock>
    {
        public async Task Consume(ConsumeContext<TriggerLock> ctx)
        {
            // Call your controller/driver here
            var now = DateTime.Now.ToString();
            var isAllowed = ctx.Message.IsAllowed;

            if (isAllowed)
            {
                await ctx.Publish(new LockTriggerGranted(ctx.Message.LockId, DateTimeOffset.UtcNow, ctx.Message.CorrelationId));
                //TODO: Add this to the new LiveEvent table with a structured record content like the message below
                //(maybe better to add to a new LiveEventsService that will add content in table and also will log  into logger
                Console.WriteLine($"[EVENT] LockTriggerGranted  LockId={ctx.Message.LockId} At={now:o} Corr={ctx.Message.CorrelationId}");
            }
            else
            {
                await ctx.Publish(new LockTriggerDenied(ctx.Message.LockId, "Device NACK", DateTimeOffset.UtcNow, ctx.Message.CorrelationId));
                Console.WriteLine($"[EVENT] LockTriggerDenied  LockId={ctx.Message.LockId} Reason=\"{ctx.Message.Reason}\" At={now:o} Corr={ctx.Message.CorrelationId}");
            }
        }
    }
}