﻿using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Helper;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.ScheduleHandlers
{
    public class GetSchedules
    {
        public class Request : IRequest<Response>
        {
            public Guid SiteId { get; set; }
        }
        public class Response
        {
            public class Item
            {
                public Guid ScheduleId { get; set; }
                public List<string> ListOfDays { get; set; } = new List<string>();
                public string DisplayName { get; set; }
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
                var schedules = await _session.Query<Schedule>()
                    .Where(x => x.SiteId == request.SiteId)
                    .ToListAsync();

                if (!schedules.Any())
                    throw new CoreException("No Schedules available");

                return new Response
                {
                    Items = schedules.Select(x => new Response.Item
                    {
                        ScheduleId = x.ScheduleId,
                        DisplayName = x.DisplayName,
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
