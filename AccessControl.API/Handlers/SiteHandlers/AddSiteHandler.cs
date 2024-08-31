using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.SiteHandlers
{
    public class AddSite
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
            public string DisplayName { get; set; }
        }
        public class Response
        {
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var site = new Site(request.UserId, request.DisplayName);

                _session.Store(site);
                await _session.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
