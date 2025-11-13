using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LockUnlock;
using AccessControl.API.Services.Infrastructure.Messaging;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockUnlockHandlers
{
    public class TriggerLockDoor
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

            public Handler(IDocumentSession session, IDomainEventDispatcher dispatcher, IAccessValidator accessValidator)
            {
                _session = session;
                _dispatcher = dispatcher;
                _accessValidator = accessValidator;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var lockToUpdate = await _session.LoadAsync<Lock>(request.LockId);
                if (lockToUpdate == null)
                    throw new CoreException("Lock not found");

                var validation = await _accessValidator.ValidateLockTriggerAsync(lockToUpdate, request.CardNumber, request.MomentaryTriggerDate);

                lockToUpdate.TriggerLock(request.CardNumber, validation.IsAllowed, validation.Reason);
                _session.Store(lockToUpdate);

                await _dispatcher.DispatchAsync(lockToUpdate.DomainEvents);
                lockToUpdate.ClearDomainEvents();

                if (!validation.IsAllowed)
                    throw new CoreException(validation.Reason ?? "Lock Trigger Denied");

                await _session.SaveChangesAsync();

                return new Response
                {
                    IsLocked = lockToUpdate.IsLocked,
                };
            }
        }
    }
}
