using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.Messaging;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.LockUnlockHandlers
{
    public class TriggerLockDoorHandler
    {
        public class TriggerLockDoor
        {
            public class Request : IRequest<Response>
            {
                public Guid LockId { get; set; }
                public DateTime MomentaryTriggerDate { get; set; }
                public int CardNumber { get; set; }
            }

            public class Response
            {
            }

            public class Handler : IRequestHandler<Request, Response>
            {
                private readonly IDocumentSession _session;
                private readonly IDomainEventDispatcher _dispatcher;

                public Handler(IDocumentSession session, IDomainEventDispatcher dispatcher)
                {
                    _session = session;
                    _dispatcher = dispatcher;
                }

                public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                {
                    var lockToUpdate = await _session.LoadAsync<Lock>(request.LockId);
                    if (lockToUpdate == null)
                        throw new CoreException("Lock not found");

                    var cardholder = await CheckIfCardNumberBelongsToAllowedUserToLock(request, lockToUpdate);

                    await CheckIfScheduleCorrespondsToAllowedUserToLock(request, lockToUpdate, cardholder);

                    lockToUpdate.TriggerLock(request.CardNumber);
                    _session.Store(lockToUpdate);
                    await _session.SaveChangesAsync();

                    await _dispatcher.DispatchAsync(lockToUpdate.DomainEvents);
                    lockToUpdate.ClearDomainEvents();

                    return new Response();
                }

                private async Task<Cardholder> CheckIfCardNumberBelongsToAllowedUserToLock(Request request, Lock lockToUpdate)
                {
                    var allowedUserIds = lockToUpdate.AllowedUsers
                        .Select(x => x.CardholderId)
                        .ToList();

                    var cardholders = await _session.Query<Cardholder>()
                        .Where(x => x.CardholderId.IsOneOf(allowedUserIds))
                        .ToListAsync();

                    var cardholder = cardholders.FirstOrDefault(x => x.ValidateCardNumber(request.CardNumber));
                    if (cardholder == null)
                        throw new CoreException($"Cardholder with the CardNumber: {request.CardNumber} is not Allowed User to Lock.");

                    return cardholder;
                }

                private async Task CheckIfScheduleCorrespondsToAllowedUserToLock(Request request, Lock lockToUpdate, Cardholder cardholder)
                {
                    var allowedUser = lockToUpdate.AllowedUsers.FirstOrDefault(x => x.CardholderId == cardholder.CardholderId);

                    var allowedUserSchedule = await _session.LoadAsync<Schedule>(allowedUser.ScheduleId);

                    if (allowedUserSchedule.StartTime >= request.MomentaryTriggerDate &&
                        allowedUserSchedule.EndTime <= request.MomentaryTriggerDate)
                        throw new CoreException($"The Lock triggered date is not corresponding to the Allowed User's dedicated Schedule.");

                    if (allowedUserSchedule.Type == ScheduleType.Standard)
                    {
                        var dayOfTrigger = request.MomentaryTriggerDate.DayOfWeek;
                        var triggerIsInAllowedUserWeekDay = allowedUserSchedule.ListOfDays.Any(x => x.Equals(dayOfTrigger));
                        if (!triggerIsInAllowedUserWeekDay)
                            throw new CoreException($"The Lock triggered week day is not corresponding to the Allowed User's dedicated Schedule.");
                    }
                }
            }
        }
    }
}
