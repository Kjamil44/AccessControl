﻿using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.AllowedUserHandlers
{
    public class AssignAccessToLock
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
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var lockFromDb = await _session.LoadAsync<Lock>(request.LockId);
                if (lockFromDb == null)
                    throw new CoreException("Lock not found");

                var cardholder = await _session.Query<Cardholder>()
                    .FirstOrDefaultAsync(x => x.SiteId == lockFromDb.SiteId && x.CardholderId == request.CardholderId);

                if (cardholder == null)
                    throw new CoreException("Cardholder not found");

                var schedule = await _session.Query<Schedule>()
                    .FirstOrDefaultAsync(x => x.SiteId == lockFromDb.SiteId && x.ScheduleId == request.ScheduleId);

                if (schedule == null)
                    throw new CoreException("Schedule not found");

                var isPresent = lockFromDb.AllowedUsers
                    .Any(x => x.CardholderId == request.CardholderId);

                if (!isPresent)
                {
                    var allowedUser = new AllowedUser
                    {
                        CardholderId = request.CardholderId,
                        ScheduleId = request.ScheduleId
                    };
                    lockFromDb.AssignAccessToLock(allowedUser);
                    _session.Store(lockFromDb);
                    await _session.SaveChangesAsync();
                }    
                return new Response();
            }
        }
    }
}
