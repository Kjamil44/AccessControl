using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;

namespace AccessControl.API.Services.Infrastructure.LockUnlock
{
    public class LockUnlockService : ILockUnlockService
    {
        private readonly IDocumentSession _session;

        public LockUnlockService(IDocumentSession session) => _session = session;

        public async Task<Cardholder> EnsureCardholderAllowedAsync(string cardNumber, Lock lockToUpdate)
        {
            var allowedUserIds = lockToUpdate.AllowedUsers
                       .Select(x => x.CardholderId)
                       .ToList();

            var cardholders = await _session.Query<Cardholder>()
                .Where(x => x.CardholderId.IsOneOf(allowedUserIds))
                .ToListAsync();

            var cardholder = cardholders.FirstOrDefault(x => x.ValidateCardNumber(cardNumber));
            if (cardholder == null)
                throw new CoreException($"Cardholder with the CardNumber: {cardNumber} is not Allowed User to Lock.");

            return cardholder;
        }

        public async Task EnsureScheduleActiveAsync(DateTime momentaryTriggerDate, Lock lockToUpdate, Cardholder cardholder)
        {
            var allowedUser = lockToUpdate.AllowedUsers.FirstOrDefault(x => x.CardholderId == cardholder.CardholderId);

            var allowedUserSchedule = await _session.LoadAsync<Schedule>(allowedUser.ScheduleId);
            if (allowedUserSchedule.Type == ScheduleType.Standard)
            {
                if (allowedUserSchedule.StartTime > momentaryTriggerDate ||
                    allowedUserSchedule.EndTime < momentaryTriggerDate)
                    throw new CoreException($"The Lock triggered date is not corresponding to the Allowed User's dedicated Schedule.");

                var dayOfTrigger = momentaryTriggerDate.DayOfWeek;
                var triggerIsInAllowedUserWeekDay = allowedUserSchedule.ListOfDays.Any(x => x.Equals(dayOfTrigger));
                if (!triggerIsInAllowedUserWeekDay)
                    throw new CoreException($"The Lock triggered week day is not corresponding to the Allowed User's dedicated Schedule.");
            }
            else if (allowedUserSchedule.Type == ScheduleType.Temporary)
            {
                var momentaryTime = momentaryTriggerDate.TimeOfDay;
                if (allowedUserSchedule.StartTime.TimeOfDay > momentaryTime ||
                    allowedUserSchedule.EndTime.TimeOfDay < momentaryTime)
                    throw new CoreException($"The Lock triggered time is not corresponding to the Allowed User's dedicated Schedule.");
            }
        }
    }
}
