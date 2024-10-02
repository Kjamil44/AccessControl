using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Helper;
using Marten;
using MediatR;
using AccessControl.API.Enums;
using Microsoft.OpenApi.Extensions;

namespace AccessControl.API.Handlers.ScheduleHandlers
{
    public class GetSchedules
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
            public Guid? SiteId { get; set; }
        }
        public class Response
        {
            public class Item
            {
                public Guid ScheduleId { get; set; }
                public List<string> ListOfDays { get; set; } = new List<string>();
                public string DisplayName { get; set; }
                public string? SiteName { get; set; }
                public string Type { get; set; }
                public DateTime StartTime { get; set; }
                public DateTime EndTime { get; set; }
                public DateTime DateModified { get; set; }
            }
            public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var sites = await _session.Query<Site>()
                    .Where(x => x.UserId == request.UserId)
                    .ToListAsync();

                var siteIds = sites.Select(x => x.SiteId).ToArray();

                var schedules = await _session
                    .Query<Schedule>()
                    .Where(x => x.SiteId.IsOneOf(siteIds))
                    .ToListAsync();

                if (!schedules.Any())
                    throw new CoreException("No Schedules available");

                if(request.SiteId.HasValue)
                    schedules = schedules.Where(x => x.SiteId == request.SiteId).ToList();

                return new Response
                {
                    Items = schedules.Select(x => new Response.Item
                    {
                        ScheduleId = x.ScheduleId,
                        DisplayName = x.DisplayName,
                        SiteName = sites.FirstOrDefault(y => y.SiteId == x.SiteId)?.DisplayName,
                        Type = x.Type.GetDisplayName(),
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        ListOfDays = HelperClass.MapWeekDays(x.ListOfDays),
                        DateModified = x.DateModified
                    })
                };
            }
        }
    }
}
