using AccessControl.API.Exceptions;
using AccessControl.API.Models;

namespace AccessControl.API.Services.Infrastructure.LockUnlock
{
    public class AccessValidator : IAccessValidator
    {
        private readonly ILockUnlockService _lockUnlockService;

        public AccessValidator(ILockUnlockService lockUnlockService)
        {
            _lockUnlockService = lockUnlockService;
        }

        public async Task<AccessValidationResult> ValidateLockTriggerAsync(Lock @lock, string cardNumber, DateTime mommentaryTriggerDate)
        {
            return await ValidateTrigger(@lock, cardNumber, mommentaryTriggerDate);
        }

        public async Task<AccessValidationResult> ValidateUnlockTriggerAsync(Lock @lock, string cardNumber, DateTime mommentaryTriggerDate)
        {
            return await ValidateTrigger(@lock, cardNumber, mommentaryTriggerDate);
        }

        private async Task<AccessValidationResult> ValidateTrigger(Lock @lock, string cardNumber, DateTime mommentaryTriggerDate)
        {
            try
            {
                var cardholder = await _lockUnlockService.EnsureCardholderAllowedAsync(cardNumber, @lock);

                await _lockUnlockService.EnsureScheduleActiveAsync(mommentaryTriggerDate, @lock, cardholder);

                return new AccessValidationResult(true);
            }
            catch (Exception ex)
            {
                return new AccessValidationResult(false, ex.Message);
            }
        }
    }
}
