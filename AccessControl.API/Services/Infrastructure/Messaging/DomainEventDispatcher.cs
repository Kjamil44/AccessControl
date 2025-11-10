using AccessControl.API.DomainEvents;
using AccessControl.Contracts;
using MassTransit;


namespace AccessControl.API.Services.Infrastructure.Messaging
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ISendEndpointProvider _sender;

        public DomainEventDispatcher(ISendEndpointProvider sender)
            => _sender = sender;

        public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
        {
            var endpoint = await _sender.GetSendEndpoint(new Uri("queue:trigger-lock"));

            foreach (var e in events)
            {
                switch (e)
                {
                    case LockTriggeredDomainEvent d:
                        await endpoint.Send(new TriggerLock(d.LockId, d.CardNumber, Guid.NewGuid()), ct);
                        break;

                    case UnlockTriggeredDomainEvent d:
                        await endpoint.Send(new TriggerUnlock(d.LockId, d.CardNumber, Guid.NewGuid()), ct);
                        break;
                }
            }
        }
    }
}
