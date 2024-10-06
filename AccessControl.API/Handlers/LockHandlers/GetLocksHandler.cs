using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockHandlers
{
    public class GetLocks
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
                public Guid LockId { get; set; }
                public string DisplayName { get; set; }
                public int NumberOfAllowedUsers { get; set; }
                public int NumberOfCardholdersPerSite { get; set; }
                public int NumberOfSchedulesPerSite { get; set; }
                public DateTime DateCreated { get; set; }
                public DateTime DateModified { get; set; }
                public string? SiteName { get; set; }
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

                var locks = await _session
                    .Query<Lock>()
                    .Where(x => x.SiteId.IsOneOf(siteIds))
                    .ToListAsync();

                if (!locks.Any())
                    throw new CoreException("No Locks available");

                if (request.SiteId.HasValue)
                    locks = locks.Where(x => x.SiteId == request.SiteId).ToList();

                var cardholders = await _session.Query<Cardholder>()
                    .ToListAsync();

                var schedules = await _session.Query<Schedule>()
                    .ToListAsync();

                return new Response
                {
                    Items = locks.Select(x => new Response.Item
                    {
                        LockId = x.LockId,
                        DisplayName = x.DisplayName,
                        NumberOfAllowedUsers = x.AllowedUsers.Count(),
                        NumberOfCardholdersPerSite = cardholders.Where(y => y.SiteId == x.SiteId).Count(),
                        NumberOfSchedulesPerSite = schedules.Where(y => y.SiteId == x.SiteId).Count(),
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        SiteName = sites.FirstOrDefault(y => y.SiteId == x.SiteId)?.DisplayName
                    })
                };
            }
        }
    }
}
