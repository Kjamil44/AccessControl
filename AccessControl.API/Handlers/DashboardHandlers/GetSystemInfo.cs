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
            public Guid UserId { get; set; }
        }
        public class Response
        {
            public IEnumerable<ChartData> AllSites { get; set; } = Enumerable.Empty<ChartData>();
            public IEnumerable<ChartData> AllLocksBySite { get; set; } = Enumerable.Empty<ChartData>();
            public IEnumerable<CardholdersByMonthInfo> CardholdersInLastSixMonths { get; set; } = Enumerable.Empty<CardholdersByMonthInfo>();
            public int NumberOfSites { get; set; }
            public int NumberOfLocks { get; set; }
            public int NumberOfCardholders { get; set; }
            public int NumberOfSchedules { get; set; }
            public int CardholdersWithAccess { get; set; }

            public class ChartData
            {
                public Guid Id { get; set; }
                public string DisplayName { get; set; }
                public int Data { get; set; }
                public string BackgroundColor { get; set; }
            }

            public class CardholdersByMonthInfo
            {
                public string MonthName { get; set; }
                public int CardholdersCount { get; set; }
                public DateTime MonthDate { get; set; }
            }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var allSites = await _session
                    .Query<Site>()
                    .Where(x => x.UserId == request.UserId)
                    .ToListAsync();

                if (!allSites.Any())
                    return new Response();

                var allSiteIds = allSites.Select(x => x.SiteId).ToArray();

                var allLocks = await _session.Query<Lock>()
                    .Where(x => x.SiteId.IsOneOf(allSiteIds))
                    .ToListAsync();

                var cardholders = await _session.Query<Cardholder>()
                    .Where(x => x.SiteId.IsOneOf(allSiteIds))
                    .ToListAsync();

                var sitesForChart = allSites.Select(x => new Response.ChartData()
                {
                    Id = x.SiteId,
                    DisplayName = x.DisplayName,
                    Data = GetNumberOfLocks(allLocks, x.SiteId),
                    BackgroundColor = ColorGenerator.GenerateRandomColor()
                });

                var siteDictionary = sitesForChart.ToDictionary(x => x.Id, x => x.DisplayName);

                var locksForChart = allLocks
                    .Where(lockItem => siteDictionary.ContainsKey(lockItem.SiteId))
                    .Select(lockItem => new Response.ChartData()
                    {
                        Id = lockItem.LockId,
                        DisplayName = $"{lockItem.DisplayName} - {siteDictionary[lockItem.SiteId]}",
                        Data = lockItem.AllowedUsers.Count(),
                        BackgroundColor = ColorGenerator.GenerateRandomColor(),
                    })
                    .ToList();

                var cardholdersCount = cardholders.Count();

                var schedulesCount = await _session.Query<Schedule>()
                    .Where(x => x.SiteId.IsOneOf(allSiteIds))
                    .CountAsync();

                var allowedUsersCount = allLocks.Select(x => x.AllowedUsers).Count();

                return new Response
                {
                    AllSites = sitesForChart,
                    AllLocksBySite = locksForChart,
                    NumberOfSites = allSites.Count(),
                    NumberOfLocks = allLocks.Count(),
                    NumberOfCardholders = cardholdersCount,
                    NumberOfSchedules = schedulesCount,
                    CardholdersWithAccess = allowedUsersCount,
                    CardholdersInLastSixMonths = GetCardholdersCountForLastSixMonths(cardholders)
                };
            }

            private int GetNumberOfLocks(IReadOnlyList<Lock> allLocks, Guid siteId)
            {
                var numOfLockPerSite = allLocks
                    .Where(x => x.SiteId == siteId)
                    .Count();

                return numOfLockPerSite;
            }

            private List<Response.CardholdersByMonthInfo> GetCardholdersCountForLastSixMonths(IReadOnlyList<Cardholder> cardholders)
            {
                var months = new List<Response.CardholdersByMonthInfo>();
                for (int i = 0; i < 6; i++)
                {
                    var date = DateTime.Now.AddMonths(-i);

                    var cardholdersCount = cardholders
                        .Where(x => x.DateCreated.Year == date.Year && x.DateCreated.Month == date.Month)
                        .Count();

                    months.Add(new Response.CardholdersByMonthInfo
                    {
                        MonthName = date.ToString("MMMM"),
                        CardholdersCount = cardholdersCount,
                        MonthDate = new DateTime(date.Year, date.Month, 1)
                    });
                }

                return months;
            }
        }
    }
}
