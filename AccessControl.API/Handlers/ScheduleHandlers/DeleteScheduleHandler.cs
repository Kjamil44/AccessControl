using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MassTransit;
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
            private readonly ILiveEventPublisher _liveEventPublisher;

            public Handler(IDocumentSession session, ILiveEventPublisher liveEventPublisher)
            {
                _session = session;
                _liveEventPublisher = liveEventPublisher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var scheduleToRemove = await _session.LoadAsync<Schedule>(request.ScheduleId);
                if (scheduleToRemove == null)
                    throw new CoreException("Schedule not found");

                var locks = await _session.Query<Lock>()
                    .ToListAsync();

                foreach (var item in locks)
                {
                    var allowedUsers = item.AllowedUsers
                        .Where(u => u.ScheduleId == request.ScheduleId)
                        .ToList();

                    if (allowedUsers != null)
                    {
                        allowedUsers.ForEach(y =>
                        {
                            item.RemoveAccessFromLock(y);
                        });
                        _session.Store(item);
                    }
                }
                _session.Delete(scheduleToRemove);

                await _liveEventPublisher.PublishAsync(
                    scheduleToRemove.SiteId,
                    scheduleToRemove.ScheduleId,
                    "Schedule",
                    LiveEventMessageType.ScheduleDeleted,
                    scheduleToRemove.DisplayName,
                    "Schedule deleted");

                await _session.SaveChangesAsync();
                return new Response();
            }
        }
    }
}
