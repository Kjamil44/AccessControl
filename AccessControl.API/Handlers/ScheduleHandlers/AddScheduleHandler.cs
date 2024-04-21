using AccessControl.API.Exceptions;
using AccessControl.API.Helper;
using AccessControl.API.Models;
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
                var schedule = new Schedule(
                    request.SiteId,
                    HelperClass.MapScheduledDays(request.ListOfDays),
                    request.DisplayName,
                    request.StartTime.ToUniversalTime(),
                    request.EndTime.ToUniversalTime());

                _session.Store(schedule);
                await _session.SaveChangesAsync();
                return new Response();
            }
        }
    }
}
