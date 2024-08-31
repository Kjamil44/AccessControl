using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.SiteHandlers
{
    public class GetSites
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
        }
        public class Response
        {
            public class Item
            {
                public Guid SiteId { get; set; }
                public string DisplayName { get; set; }
                public DateTime DateCreated { get; set; }
                public DateTime DateModified { get; set; }
            }
            public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var sites = await _session
                    .Query<Site>()
                    .Where(x => x.UserId == request.UserId)
                    .ToListAsync();

                if (!sites.Any())
                    throw new CoreException("No Sites available");

                return new Response
                {
                    Items = sites.Select(x => new Response.Item
                    {
                        SiteId = x.SiteId,
                        DisplayName = x.DisplayName,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified
                    })
                };
            }
        }
    }
}
