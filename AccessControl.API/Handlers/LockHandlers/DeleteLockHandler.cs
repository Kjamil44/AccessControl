using AccessControl.API.Exceptions;
using AccessControl.API.Models;
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
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {   
                var lockToRemove = await _session.LoadAsync<Lock>(request.LockId);
                if (lockToRemove == null)
                    throw new CoreException("Lock not found");

                _session.Delete(lockToRemove);  
                await _session.SaveChangesAsync();
                return new Response();
            }
        }
    }

}
