using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.UserHandlers
{
    public class GetCurrentUser
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
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
                var user = await _session.LoadAsync<User>(request.UserId);
                if (user == null)
                    throw new CoreException("Schedule not found");

                return new Response
                {
                    Email = user.Email,
                    Username = user.Username,
                };
            }
        }
    }
}
