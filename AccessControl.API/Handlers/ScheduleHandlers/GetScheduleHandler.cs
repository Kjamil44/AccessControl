using AccessControl.API.Exceptions;
using AccessControl.API.Helpers;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.ScheduleHandlers
{
    public class GetSchedule
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
            public Guid ScheduleId { get; set; }
        }
        public class Response
        {
            public Guid ScheduleId { get; set; }
            public List<string> ListOfDays { get; set; } = new List<string>();
            public string DisplayName { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public DateTime DateModified { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var schedule = await _session.LoadAsync<Schedule>(request.ScheduleId);
                if (schedule == null)
                    throw new CoreException("Schedule not found");

                return new Response
                {
                    ScheduleId = schedule.ScheduleId,
                    DisplayName = schedule.DisplayName,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                    ListOfDays = HelperClass.MapWeekDays(schedule.ListOfDays),
                    DateModified = schedule.DateModified
                };
            }
        }
    }
}
