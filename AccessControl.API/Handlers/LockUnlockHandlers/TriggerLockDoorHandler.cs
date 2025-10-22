using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.Messaging;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockUnlockHandlers
{
    public class TriggerLockDoorHandler
    {
        public class TriggerLockDoor
        {
            public class Request : IRequest<Response>
            {
                public Guid LockId { get; set; }
                public int CardNumber { get; set; }
            }

            public class Response
            {
            }

            public class Handler : IRequestHandler<Request, Response>
            {
                private readonly IDocumentSession _session;
                private readonly IDomainEventDispatcher _dispatcher;

                public Handler(IDocumentSession session, IDomainEventDispatcher dispatcher)
                {
                    _session = session;
                    _dispatcher = dispatcher;
                }

                public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                {
                    var lockToUpdate = await _session.LoadAsync<Lock>(request.LockId);
                    if (lockToUpdate == null)
                        throw new CoreException("Lock not found");

                    lockToUpdate.TriggerLock(request.CardNumber);
                    _session.Store(lockToUpdate);
                    await _session.SaveChangesAsync();

                    await _dispatcher.DispatchAsync(lockToUpdate.DomainEvents);
                    lockToUpdate.ClearDomainEvents();
                    
                    return new Response();
                }
            }
        }
    }
}
