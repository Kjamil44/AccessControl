using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Helpers;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.ScheduleHandlers
{
    public class UpdateSchedule
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public Guid ScheduleId { get; set; }
            public List<string> ListOfDays { get; set; } = new List<string>();
            public string DisplayName { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
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
                var schedule = await _session.LoadAsync<Schedule>(request.ScheduleId);
                if (schedule == null)
                    throw new CoreException("Schedule not found");

                if (!request.ListOfDays.Any())
                    request.ListOfDays = HelperClass.MapWeekDays(schedule.ListOfDays);

                if (request.DisplayName.IsEmpty())
                    request.DisplayName = schedule.DisplayName;

                schedule.UpdateSchedule(
                    HelperClass.MapScheduledDays(request.ListOfDays),
                    request.DisplayName,
                    request.StartTime.ToUniversalTime(),
                    request.EndTime.ToUniversalTime());

                _session.Store(schedule);

                await _liveEventPublisher.PublishAsync(
                     schedule.SiteId,
                     schedule.ScheduleId,
                     "Schedule",
                     LiveEventMessageType.ScheduleDeleted,
                     schedule.DisplayName,
                     "Schedule updated");

                await _session.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
