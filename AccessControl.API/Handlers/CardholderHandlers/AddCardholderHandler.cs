using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.CardholderHandlers
{
    public class AddCardholder
    {
        public class Request : ICommand<Response>
        {
            public Guid SiteId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CardNumber { get; set; }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            private readonly ILiveEventPublisher _liveEventPublisher;

            public Handler(IDocumentSession session, ILiveEventPublisher liveEventPublisher)
            {
                _session = session;
                _liveEventPublisher = liveEventPublisher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var cardholder = new Cardholder(request.SiteId, request.FirstName, request.LastName, request.CardNumber);

                var alreadyAdded = await _session.Query<Cardholder>()
                    .AnyAsync(x => x.SiteId == request.SiteId &&
                              x.FirstName.Equals(request.FirstName) &&
                              x.LastName.Equals(request.LastName) &&
                              x.CardNumber == request.CardNumber);

                if (alreadyAdded)
                    throw new CoreException("Cardholder Already Added");

                _session.Store(cardholder);

                await _liveEventPublisher.PublishAsync(
                    cardholder.SiteId,
                    cardholder.CardholderId,
                    "Cardholder",
                    LiveEventMessageType.CardholderCreated,
                    cardholder.FullName,
                    "Cardholder created");

                return new Response();
            }
        }
    }
}
