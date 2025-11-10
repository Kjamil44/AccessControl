using AccessControl.API.Exceptions;
using AccessControl.API.Models;
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
            ILockUnlockService _lockUnlockService;

            public Handler(IDocumentSession session, IDomainEventDispatcher dispatcher, ILockUnlockService lockUnlockService)
            {
                _session = session;
                _dispatcher = dispatcher;
                _lockUnlockService = lockUnlockService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var lockToUpdate = await _session.LoadAsync<Lock>(request.LockId);
                if (lockToUpdate == null)
                    throw new CoreException("Lock not found");

                var cardholder = await _lockUnlockService.EnsureCardholderAllowedAsync(request.CardNumber, lockToUpdate);

                await _lockUnlockService.EnsureScheduleActiveAsync(request.MomentaryTriggerDate, lockToUpdate, cardholder);

                lockToUpdate.TriggerUnlock(request.CardNumber);
                _session.Store(lockToUpdate);
                await _session.SaveChangesAsync();

                await _dispatcher.DispatchAsync(lockToUpdate.DomainEvents);
                lockToUpdate.ClearDomainEvents();

                return new Response
                {
                    IsLocked = lockToUpdate.IsLocked,
                };
            }
        }
    }
}
