using AccessControl.API.Models;
using Marten;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace AccessControl.API.Handlers.UserHandlers
{
    public class GetCurrentUser
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
        }
        public class Response
        {
            public string Email { get; set; }
            public string Username { get; set; }
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
                var userId = Guid.Parse(GetUserIdFromToken());

                var user = _session.Query<User>().FirstOrDefault(x => x.Id == userId);

                return new Response
                {
                    Email = user.Email,
                    Username = user.Username,
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
