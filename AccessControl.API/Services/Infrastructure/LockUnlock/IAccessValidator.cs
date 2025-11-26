using AccessControl.API.Exceptions;
using AccessControl.API.Models;

namespace AccessControl.API.Services.Infrastructure.LockUnlock
{
    public interface IAccessValidator
    {
        public Task<AccessValidationResult> ValidateLockTriggerAsync(Lock @lock, string cardNumber, DateTime mommentaryTriggerDate);
        public Task<AccessValidationResult> ValidateUnlockTriggerAsync(Lock @lock, string cardNumber, DateTime mommentaryTriggerDate);
    }
}

