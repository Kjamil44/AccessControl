using AccessControl.API.DomainEvents;


namespace AccessControl.API.Services.Infrastructure.Messaging
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
    }
}
