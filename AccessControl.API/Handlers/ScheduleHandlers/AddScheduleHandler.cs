using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Helpers;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.ScheduleHandlers
{
    public class AddSchedule
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public List<string> ListOfDays { get; set; } = new List<string>();
            public string DisplayName { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool IsTemporary { get; set; }
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
                var schedule = new Schedule();

                if (request.IsTemporary)
                {
                    schedule.CreateTemporary(
                        request.SiteId,
                        request.DisplayName,
                        request.StartTime,
                        request.EndTime);
                }
                else
                {
                    schedule.CreateStandard(
                        request.SiteId,
                        HelperClass.MapScheduledDays(request.ListOfDays),
                        request.DisplayName,
                        request.StartTime,
                        request.EndTime);
                }

                _session.Store(schedule);

                await _liveEventPublisher.PublishAsync(
                    schedule.SiteId,
                    schedule.ScheduleId,
                    "Schedule",
                    LiveEventMessageType.ScheduleCreated,
                    schedule.DisplayName,
                    $"{(request.IsTemporary ? "Temporary" : "Standard")} Schedule was created");

                await _session.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
