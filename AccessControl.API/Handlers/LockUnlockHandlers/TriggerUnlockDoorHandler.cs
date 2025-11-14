using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using AccessControl.API.Services.Infrastructure.LockUnlock;
using AccessControl.API.Services.Infrastructure.Messaging;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockUnlockHandlers
{
    public class TriggerUnlockDoor
    {
        public class Request : IRequest<Response>
        {
            public Guid LockId { get; set; }
            public DateTime MomentaryTriggerDate { get; set; }
            public string CardNumber { get; set; }
        }

        public class Response
        {
            public bool IsLocked { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            private readonly IDomainEventDispatcher _dispatcher;
            private readonly IAccessValidator _accessValidator;
            private readonly ILiveEventPublisher _liveEventPublisher;

            public Handler(IDocumentSession session, IDomainEventDispatcher dispatcher, 
                IAccessValidator accessValidator, ILiveEventPublisher liveEventPublisher)
            {
                _session = session;
                _dispatcher = dispatcher;
                _accessValidator = accessValidator;
                _liveEventPublisher = liveEventPublisher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var lockToUpdate = await _session.LoadAsync<Lock>(request.LockId);
                if (lockToUpdate == null)
                    throw new CoreException("Lock not found");

                var validation = await _accessValidator.ValidateUnlockTriggerAsync(lockToUpdate, request.CardNumber, request.MomentaryTriggerDate);

                lockToUpdate.TriggerUnlock(request.CardNumber, validation.IsAllowed, validation.Reason);
                _session.Store(lockToUpdate);

                await _dispatcher.DispatchAsync(lockToUpdate.DomainEvents);
                lockToUpdate.ClearDomainEvents();

                await PublishLiveEvent(lockToUpdate, validation);

                if (!validation.IsAllowed)
                    throw new CoreException(validation.Reason ?? "Unlock Trigger Denied");

                await _session.SaveChangesAsync();

                return new Response
                {
                    IsLocked = lockToUpdate.IsLocked,
                };
            }

            async Task PublishLiveEvent(Lock lockToUpdate, AccessValidationResult validation)
            {
                var liveEventMessageType = validation.IsAllowed ? LiveEventMessageType.UnlockTriggerGranted : LiveEventMessageType.UnlockTriggerDenied;
                string liveEventMessage = $"Unlock Trigger is {(validation.IsAllowed ? "granted" : "denied")}";

                await _liveEventPublisher.PublishAsync(lockToUpdate.SiteId,
                    lockToUpdate.LockId,
                    "Lock",
                    liveEventMessageType,
                    lockToUpdate.DisplayName,
                    liveEventMessage);
            }
        }
    }
}
