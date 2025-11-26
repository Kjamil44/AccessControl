using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.CardholderHandlers
{
    public class UpdateCardholder
    {
        public class Request : ICommand<Response>
        {
            public Guid SiteId { get; set; }
            public Guid CardholderId { get; set; }
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

                await _liveEventPublisher.PublishAsync(
                    cardholder.SiteId,
                    cardholder.CardholderId,
                    "Cardholder",
                    LiveEventMessageType.CardholderUpdated,
                    cardholder.FullName,
                    "Cardholder was updated");

                return new Response();
            }
        }
    }
}
