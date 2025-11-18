using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MassTransit;
using MediatR;

namespace AccessControl.API.Handlers.AllowedUserHandlers
{
    public class EditAccessToLock
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public Guid LockId { get; set; }
            public Guid CardholderId { get; set; }
            public Guid ScheduleId { get; set; }
        }
        public class Response
        {
            public Guid ScheduleId { get; set; }
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
                var lockFromDb = await _session.LoadAsync<Lock>(request.LockId);
                if (lockFromDb == null)
                    throw new CoreException("Lock not found");

                var cardholder = await _session.Query<Cardholder>()
                    .FirstOrDefaultAsync(x => x.CardholderId == request.CardholderId);

                if (cardholder == null)
                    throw new CoreException("Cardholder not found");

                var allowedUser = lockFromDb.AllowedUsers
                    .FirstOrDefault(x => x.CardholderId == request.CardholderId);

                if (allowedUser == null)
                    throw new CoreException("No Assigned Users for Cardholder");

                allowedUser.ScheduleId = request.ScheduleId;

                lockFromDb.EditAccessToLock(allowedUser);
                _session.Store(lockFromDb);

                await _liveEventPublisher.PublishAsync(
                    lockFromDb.SiteId,
                    lockFromDb.LockId,
                    "Lock",
                    LiveEventMessageType.LockAccessListUpdated,
                    lockFromDb.DisplayName,
                    $"Updated schedule for {cardholder.FullName} (lock access).");

                await _session.SaveChangesAsync();

                return new Response
                {
                    ScheduleId = allowedUser.ScheduleId
                };
            }
        }
    }
}
