using AccessControl.API.Enums;

namespace AccessControl.API.Services.Infrastructure.LiveEvents
{
    public interface ILiveEventPublisher
    {
       public Task PublishAsync(Guid siteId, Guid entityId, string entityType, LiveEventMessageType messageType, string name, string message);
    }
}
