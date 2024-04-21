using AccessControl.API.Exceptions;
using AccessControl.API.Models;
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
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var cardholder = await _session.LoadAsync<Cardholder>(request.CardholderId);

                if (cardholder == null)
                    throw new CoreException("Cardholder not found");

                var locks = await _session.Query<Lock>()
                    .ToListAsync();

                locks.ToList().ForEach(x =>
                {
                    var allowedUser = x.AllowedUsers
                     .FirstOrDefault(u => u.CardholderId == request.CardholderId);

                    if (allowedUser != null)
                    {
                        x.RemoveAccessFromLock(allowedUser);
                        _session.Store(x);
                    }
                });

                _session.Delete(cardholder);
                await _session.SaveChangesAsync();
                return new Response();
            }
        }
    }
}
