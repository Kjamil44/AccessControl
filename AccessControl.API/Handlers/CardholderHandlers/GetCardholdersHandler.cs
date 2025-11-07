using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.CardholderHandlers
{
    public class GetCardholders
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
            public Guid? SiteId { get; set; }
        }
        public class Response
        {
            public class Item
            {
                public Guid CardholderId { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string FullName { get; set; }
                public string? SiteName { get; set; }
                public string CardNumber { get; set; }
                public DateTime ActivationDate { get; set; }
                public DateTime ExpirationDate { get; set; }
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
                var sites = await _session.Query<Site>()
                    .Where(x => x.UserId == request.UserId)
                    .ToListAsync();

                var siteIds = sites.Select(x => x.SiteId).ToArray();

                var cardholders = await _session
                    .Query<Cardholder>()
                    .Where(x => x.SiteId.IsOneOf(siteIds))
                    .ToListAsync();

                if (!cardholders.Any())
                    throw new CoreException("No Cardholders available");

                if (request.SiteId.HasValue)
                    cardholders = cardholders.Where(x => x.SiteId == request.SiteId).ToList();


                return new Response
                {
                    Items = cardholders.Select(x => new Response.Item
                    {
                        CardholderId = x.CardholderId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,  
                        FullName = x.FullName,
                        SiteName = sites.FirstOrDefault(y => y.SiteId == x.SiteId)?.DisplayName,
                        CardNumber = x.CardNumber,  
                        ActivationDate = x.ActivationDate,  
                        ExpirationDate = x.ExpirationDate,  
                        DateModified = x.DateModified
                    })
                };
            }
        }
    }
}
