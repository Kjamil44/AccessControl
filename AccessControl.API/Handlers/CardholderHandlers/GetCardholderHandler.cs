using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.CardholderHandlers
{
    public class GetCardholder
    {
        public class Request : IRequest<Response>
        {
            public Guid CardholderId { get; set; }
        }
        public class Response
        {
            public Guid CardholderId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
            public int CardNumber { get; set; }
            public DateTime ActivationDate { get; set; }
            public DateTime ExpirationDate { get; set; }
            public DateTime DateModified { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var cardholder = await _session.LoadAsync<Cardholder>(request.CardholderId);
                if (cardholder == null)
                    throw new CoreException("Carholder not found");

                return new Response
                {
                    CardholderId = cardholder.CardholderId,
                    FirstName = cardholder.FirstName,
                    LastName = cardholder.LastName,
                    FullName = cardholder.FullName,
                    CardNumber = cardholder.CardNumber,
                    ActivationDate = cardholder.ActivationDate,
                    ExpirationDate = cardholder.ExpirationDate,
                    DateModified = cardholder.DateModified,
                };
            }
        }
    }
}
