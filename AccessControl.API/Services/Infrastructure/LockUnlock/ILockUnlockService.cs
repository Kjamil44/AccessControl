using AccessControl.API.Models;

namespace AccessControl.API.Services.Infrastructure.LockUnlock
{
    public interface ILockUnlockService
    {
        public Task<Cardholder> EnsureCardholderAllowedAsync(string cardNumber, Lock lockToUpdate);
        public Task EnsureScheduleActiveAsync(DateTime momentaryTriggerDate, Lock lockToUpdate, Cardholder cardholder);
    }
}
