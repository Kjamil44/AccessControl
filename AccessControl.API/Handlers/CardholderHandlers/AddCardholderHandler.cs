using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.CardholderHandlers
{
    public class AddCardholder
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int CardNumber { get; set; }
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
                var cardholder = new Cardholder(request.SiteId, request.FirstName, request.LastName, request.CardNumber);

                var alreadyAdded = await _session.Query<Cardholder>()
                    .AnyAsync(x => x.SiteId == request.SiteId && x.FirstName.Equals(request.FirstName) && x.LastName.Equals(request.LastName) && x.CardNumber == request.CardNumber);

                if (alreadyAdded)
                    throw new CoreException("Cardholder Already Added");

                _session.Store(cardholder);
                await _session.SaveChangesAsync();
                return new Response();
            }
        }
    }
}
