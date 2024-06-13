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
            public IEnumerable<ChartData> AllCardholdersBySite { get; set; } = Enumerable.Empty<ChartData>();
            public IEnumerable<ChartData> AllSchedulesBySite { get; set; } = Enumerable.Empty<ChartData>();

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

                //var allSiteIds = allSites.Select(x => x.SiteId).ToArray();

                //var allLocks = await _session.Query<Lock>()
                //    .Where(x => x.SiteId.IsOneOf(allSiteIds))
                //    .ToListAsync();

                //var locksForChart = new List<Response.ChartData>();
                //foreach (var site in sitesForChart)
                //{
                //    foreach (var item in allLocks)
                //    {
                //        if (site.Id == item.SiteId)
                //        {
                //            locksForChart.Add(new Response.ChartData()
                //            {
                //                Id = item.LockId,
                //                DisplayName = item.DisplayName,
                //                BackgroundColor = site.BackgroundColor,
                //            });
                //        }
                //    }
                //}


                return new Response
                {
                    AllSites = sitesForChart,
                };
            }
        }
    }
}
