using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockHandlers
{
    public class GetLock
    {
        public class Request : IRequest<Response>
        {
            public Guid LockId { get; set; }
        }
        public class Response
        {
            public Guid LockId { get; set; }
            public Guid SiteId { get; set; }
            public string SiteDisplayName { get; set; }
            public string DisplayName { get; set; }
            public class Item
            {
                public Guid CardholderId { get; set; }
                public string CardholderName { get; set; }
                public string ScheduleName { get; set; }
                public List<Days> ScheduleDays { get; set; } = new List<Days>();
            }
            public IEnumerable<Item> AssignedUsers { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime DateModified { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;

            public Handler(IDocumentSession session) => _session = session;
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var lockFromDb = await _session.Query<Lock>()
                    .FirstOrDefaultAsync(x => x.LockId == request.LockId);

                if (lockFromDb == null)
                    throw new CoreException("Lock not found");

                var site = await _session.LoadAsync<Site>(lockFromDb.SiteId);
                if (site == null)
                    throw new CoreException("Site not found");

                var cardholders = await _session.Query<Cardholder>().ToListAsync();
                var schedules = await _session.Query<Schedule>().ToListAsync();

                return new Response
                {
                    LockId = lockFromDb.LockId,
                    SiteId = site.SiteId,
                    SiteDisplayName = site.DisplayName,
                    DisplayName = lockFromDb.DisplayName,
                    AssignedUsers = lockFromDb.AllowedUsers.Select(x =>
                    {
                        var cardholder = cardholders.FirstOrDefault(c => c.CardholderId == x.CardholderId);
                        var schedule = schedules.FirstOrDefault(s => s.ScheduleId == x.ScheduleId);
                        if (cardholder == null || schedule == null)
                        {
                            return new Response.Item
                            {
                            };
                        }
                        return new Response.Item
                        {
                            CardholderId = cardholder.CardholderId,
                            CardholderName = $"{cardholder.FirstName} {cardholder.LastName}",
                            ScheduleName = schedule.DisplayName,
                            ScheduleDays = schedule.ListOfDays.ToList()
                        };
                    }),
                    DateCreated = lockFromDb.DateCreated,
                    DateModified = lockFromDb.DateModified
                };
            }
        }
    }
}
