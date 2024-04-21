using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.CardholderHandlers
{
    public class UpdateCardholder
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public Guid CardholderId { get; set; }
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
                var cardholder = await _session.LoadAsync<Cardholder>(request.CardholderId);

                if (cardholder == null)
                    throw new CoreException("Cardholder not found");

                if (request.FirstName.IsEmpty())
                    request.FirstName = cardholder.FirstName;

                if (request.LastName.IsEmpty())
                    request.LastName = cardholder.LastName;

                if (request.CardNumber.ToString().IsEmpty())
                    request.CardNumber = cardholder.CardNumber;

                cardholder.UpdateCardholder(request.FirstName, request.LastName, request.CardNumber);
                _session.Store(cardholder);
                await _session.SaveChangesAsync();
                return new Response();
            }
        }
    }
}
