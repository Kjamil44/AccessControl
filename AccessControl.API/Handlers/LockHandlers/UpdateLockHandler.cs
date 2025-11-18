using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockHandlers
{
    public class UpdateLock
    {
        public class Request : IRequest<Response>
        {
            public Guid LockId { get; set; }
            public string DisplayName { get; set; }
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
                var lockToUpdate = await _session.LoadAsync<Lock>(request.LockId);
                if (lockToUpdate == null)
                    throw new CoreException("Lock not found");

                lockToUpdate.UpdateLock(request.DisplayName);
                _session.Store(lockToUpdate);

                await _liveEventPublisher.PublishAsync(
                    lockToUpdate.SiteId,
                    lockToUpdate.LockId,
                    "Lock",
                    LiveEventMessageType.LockUpdated,
                    lockToUpdate.DisplayName,
                    "Lock name was updated");

                await _session.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
