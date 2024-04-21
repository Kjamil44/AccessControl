using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockHandlers
{
    public class UpdateLock
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public Guid LockId { get; set; }
            public string DisplayName { get; set; }
        }
        public class Response
        {
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var lockToUpdate = await _session.LoadAsync<Lock>(request.LockId);
                if (lockToUpdate == null)
                    throw new CoreException("Lock not found");

                lockToUpdate.UpdateLock(request.DisplayName);
                _session.Store(lockToUpdate);
                await _session.SaveChangesAsync();
                return new Response();
            }
        }
    }
}
