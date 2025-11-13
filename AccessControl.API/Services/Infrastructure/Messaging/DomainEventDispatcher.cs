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
            foreach (var e in events)
            {
                switch (e)
                {
                    case LockTriggeredDomainEvent d:
                        await _sender.Send(new TriggerLock(d.LockId, d.CardNumber, Guid.NewGuid(), d.IsAllowed), ct);
                        break;

                    case UnlockTriggeredDomainEvent d:
                        await _sender.Send(new TriggerUnlock(d.LockId, d.CardNumber, Guid.NewGuid(), d.IsAllowed), ct);
                        break;
                }
            }
        }
    }
}
