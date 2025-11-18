using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockHandlers
{
    public class DeleteLock
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public Guid LockId { get; set; }
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
                var lockToRemove = await _session.LoadAsync<Lock>(request.LockId);
                if (lockToRemove == null)
                    throw new CoreException("Lock not found");

                _session.Delete(lockToRemove);

                await _liveEventPublisher.PublishAsync(
                    lockToRemove.SiteId,
                    lockToRemove.LockId,
                    "Lock",
                    LiveEventMessageType.LockDeleted,
                    lockToRemove.DisplayName,
                    "Lock deleted");

                await _session.SaveChangesAsync();
                
                return new Response();
            }
        }
    }

}
