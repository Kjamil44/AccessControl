using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.ScheduleHandlers
{
    public class DeleteSchedule
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
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
                var scheduleToRemove = await _session.LoadAsync<Schedule>(request.ScheduleId);
                if (scheduleToRemove == null)
                    throw new CoreException("Schedule not found");

                var locks = await _session.Query<Lock>()
                    .ToListAsync();

                locks.ToList().ForEach(x =>
                {
                    var allowedUsers = x.AllowedUsers
                     .Where(u => u.ScheduleId == request.ScheduleId)
                     .ToList();

                    if (allowedUsers != null)
                    {
                        allowedUsers.ForEach(y =>
                        {
                            x.RemoveAccessFromLock(y);
                        });
                        _session.Store(x);
                    }
                });

                _session.Delete(scheduleToRemove);
                await _session.SaveChangesAsync();
                return new Response();
            }
        }
    }
}
