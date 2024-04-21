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
            public Guid SiteId { get; set; }
        }
        public class Response
        {
            public class Item
            {
                public Guid LockId { get; set; }
                public string DisplayName { get; set; }
                public class AssignedUser
                {
                    public Guid CardholderId { get; set; }
                    public string CardholderName { get; set; }
                    public string ScheduleName { get; set; }
                    public List<Days> ScheduleDays { get; set; } = new List<Days>();
                }
                public DateTime DateCreated { get; set; }
                public IEnumerable<AssignedUser> AssignedUsers { get; set; }
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
                var locks = await _session.Query<Lock>()
                    .Where(x => x.SiteId == request.SiteId)
                    .ToListAsync();

                if (!locks.Any())
                    throw new CoreException("No Locks available");

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
                        AssignedUsers = x.AllowedUsers.Select(y =>
                        {
                            var cardholder = cardholders.FirstOrDefault(c => c.CardholderId == y.CardholderId);
                            var schedule = schedules.FirstOrDefault(s => s.ScheduleId == y.ScheduleId);
                            if (cardholder == null || schedule == null)
                            {
                                //x.RemoveAccessFromLock(y);
                                return new Response.Item.AssignedUser { };
                            }
                            return new Response.Item.AssignedUser
                            {
                                CardholderId = cardholder.CardholderId,
                                CardholderName = $"{cardholder.FirstName} {cardholder.LastName}",
                                ScheduleName = schedule.DisplayName,
                                ScheduleDays = schedule.ListOfDays.ToList()
                            };
                        }),
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified
                    })
                };
            }
        }
    }
}
