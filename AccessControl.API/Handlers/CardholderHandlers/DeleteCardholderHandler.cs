using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.CardholderHandlers
{
    public class DeleteCardholder
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public Guid CardholderId { get; set; }
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

                var locks = await _session.Query<Lock>()
                    .ToListAsync();

                foreach (var item in locks)
                {
                    var allowedUser = item.AllowedUsers
                        .FirstOrDefault(u => u.CardholderId == request.CardholderId);

                    if (allowedUser != null)
                    {
                        item.RemoveAccessFromLock(allowedUser);
                        _session.Store(item);
                    }
                }

                _session.Delete(cardholder);

                await _liveEventPublisher.PublishAsync(
                    cardholder.SiteId,
                    cardholder.CardholderId,
                    "Cardholder",
                    LiveEventMessageType.CardholderDeleted,
                    cardholder.FullName,
                    "Cardholder deleted");

                await _session.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
