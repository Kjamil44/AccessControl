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
            public Guid SiteId { get; set; }
        }
        public class Response
        {
            public class Item
            {
                public Guid CardholderId { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public int CardNumber { get; set; }
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
                var cardholders = await _session.Query<Cardholder>()
                    .Where(x => x.SiteId == request.SiteId)
                    .ToListAsync();

                if (!cardholders.Any())
                    throw new CoreException("No Cardholders available");

                return new Response
                {
                    Items = cardholders.Select(x => new Response.Item
                    {
                        CardholderId = x.CardholderId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,  
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
