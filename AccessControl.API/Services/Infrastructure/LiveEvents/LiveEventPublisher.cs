using AccessControl.API.Enums;
using AccessControl.API.Helpers;
using AccessControl.API.Models;
using AccessControl.API.SignalR;
using Marten;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

namespace AccessControl.API.Services.Infrastructure.LiveEvents
{
    public class LiveEventPublisher : ILiveEventPublisher
    {
        private readonly IDocumentSession _session;
        private readonly IHubContext<LiveEventsHub> _hub;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LiveEventPublisher(IDocumentSession session, IHubContext<LiveEventsHub> hub, IHttpContextAccessor httpContextAccessor)
        {
            _session = session;
            _hub = hub;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task PublishAsync(Guid siteId, Guid entityId, string entityType,
            LiveEventMessageType messageType, string name, string message)
        {
            var userId = GetUserIdFromToken();

            var liveEvent = new LiveEvent
            {
                UserId = Guid.Parse(userId),
                SiteId = siteId,
                EntityId = entityId,
                EntityType = entityType,
                LiveEventId = Guid.NewGuid(),
                Type = messageType.ToCategory(),
                Name = name,
                Message = message,
                MessageType = messageType,
                DateCreated = DateTime.UtcNow
            };

            _session.Store(liveEvent);
            await _session.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("liveEvent", new
            {
                liveEvent.LiveEventId,
                liveEvent.SiteId,
                liveEvent.EntityId,
                liveEvent.EntityType,
                liveEvent.Type,
                liveEvent.Name,
                liveEvent.Message,
                liveEvent.MessageType,
                liveEvent.DateCreated
            });
        }

        private string GetUserIdFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                return null;
            }

            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var userId = jwtToken?.Claims.First(claim => claim.Type == "sub")?.Value;

            return userId;
        }
    }
}
