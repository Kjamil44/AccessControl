using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using Marten.Schema;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace AccessControl.API.Handlers.LiveEventHandlers
{
    public class GetLiveEvents
    {
        public class Request : IRequest<Response>
        {

        }
        public class Response
        {
            public class Item
            {
                public Guid SiteId { get; set; }
                public Guid EntityId { get; set; }
                public Guid LiveEventId { get; set; }
                public string EntityType { get; set; }
                public LiveEventType Type { get; set; }
                public string Name { get; set; }
                public string Message { get; set; }
                public LiveEventMessageType MessageType { get; set; }
                public DateTime DateCreated { get; set; }
            }

            public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IDocumentSession session, IHttpContextAccessor httpContextAccessor) 
            {
                _session = session;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var userId = GetUserIdFromToken();

                var liveEvents = await _session.Query<LiveEvent>()
                    .Where(x => x.UserId == Guid.Parse(userId))
                    .ToListAsync();

                return new Response
                {
                    Items = liveEvents.Select(x => new Response.Item
                    {
                        LiveEventId = x.LiveEventId,
                        SiteId = x.SiteId,
                        EntityId = x.EntityId,
                        EntityType = x.EntityType.ToString(),
                        Type = x.Type,
                        Name = x.Name,
                        Message = x.Message,
                        MessageType = x.MessageType,
                        DateCreated = x.DateCreated,
                    })
                };
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
}
