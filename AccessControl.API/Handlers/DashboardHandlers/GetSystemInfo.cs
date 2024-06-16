using AccessControl.API.Models;
using Marten;
using MediatR;
using static AccessControl.API.Helper.HelperClass;

namespace AccessControl.API.Handlers.DashboardHandlers
{
    public class GetSystemInfo
    {
        public class Request : IRequest<Response>
        {
        }
        public class Response
        {
            public IEnumerable<ChartData> AllSites { get; set; } = Enumerable.Empty<ChartData>();
            public IEnumerable<ChartData> AllLocksBySite { get; set; } = Enumerable.Empty<ChartData>();
            public int NumberOfCardholders { get; set; }
            public int NumberOfSchedules { get; set; }
            public int CardholdersWithAccess { get; set; }

            public class ChartData
            {
                public Guid Id { get; set; }
                public string DisplayName { get; set; }
                public string BackgroundColor { get; set; }
            }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var allSites = await _session.Query<Site>().ToListAsync();
                if (!allSites.Any())
                    return new Response();

                var sitesForChart = allSites.Select(x => new Response.ChartData()
                {
                    Id = x.SiteId,
                    DisplayName = x.DisplayName,
                    BackgroundColor = ColorGenerator.GenerateRandomColor()
                });

                var allSiteIds = allSites.Select(x => x.SiteId).ToArray();

                var allLocks = await _session.Query<Lock>()
                    .Where(x => x.SiteId.IsOneOf(allSiteIds))
                    .ToListAsync();

                var siteDictionary = sitesForChart.ToDictionary(x => x.Id, x => x.DisplayName);

                var locksForChart = allLocks
                    .Where(lockItem => siteDictionary.ContainsKey(lockItem.SiteId))
                    .Select(lockItem => new Response.ChartData()
                    {
                        Id = lockItem.LockId,
                        DisplayName = $"{lockItem.DisplayName} - {siteDictionary[lockItem.SiteId]}",
                        BackgroundColor = ColorGenerator.GenerateRandomColor(),
                    })
                    .ToList();

                var cardholdersCount = await _session.Query<Cardholder>()
                    .Where(x => x.SiteId.IsOneOf(allSiteIds))
                    .CountAsync();

                var schedulesCount = await _session.Query<Schedule>()
                    .Where(x => x.SiteId.IsOneOf(allSiteIds))
                    .CountAsync();

                var allowedUsersCount = allLocks.Select(x => x.AllowedUsers).Count();

                return new Response
                {
                    AllSites = sitesForChart,
                    AllLocksBySite = locksForChart,
                    NumberOfCardholders = cardholdersCount,
                    NumberOfSchedules = schedulesCount,
                    CardholdersWithAccess = allowedUsersCount,
                };
            }
        }
    }
}
