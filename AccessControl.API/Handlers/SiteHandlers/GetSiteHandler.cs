using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.SiteHandlers
{
    public class GetSite
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
        }
        public class Response
        {
            public Guid SiteId { get; set; }
            public string DisplayName { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime DateModified { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var site = await _session.LoadAsync<Site>(request.SiteId);
                if (site == null)
                    throw new CoreException("Site not found");

                return new Response
                {
                    SiteId = site.SiteId,
                    DisplayName = site.DisplayName,
                    DateCreated = site.DateCreated,
                    DateModified = site.DateModified
                };
            }
        }
    }
}
