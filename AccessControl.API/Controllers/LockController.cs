using AccessControl.API.Handlers.AllowedUserHandlers;
using AccessControl.API.Handlers.LockHandlers;
using AccessControl.API.Handlers.LockUnlockHandlers;
using AccessControl.API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [Route("api/locks")]
    [Authorize]
    [ApiController]
    public class LockController : ControllerBase
    {
        private readonly ISender _sender;
        public LockController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<GetLocks.Response> GetLocks([FromQuery] GetLocks.Request request, Guid userId)
        {
            request.UserId = userId;
            return await _sender.Send(request);
        }

        [HttpGet("{lockId}")]
        public async Task<GetLock.Response> GetLock(Guid lockId) => await _sender.Send(new GetLock.Request { LockId = lockId });

        [HttpPost]
        public async Task<AddLock.Response> CreateLock( [FromBody] AddLock.Request newLock) => await _sender.Send(newLock);

        [HttpPut("{lockId}")]
        public async Task<UpdateLock.Response> UpdateLock(Guid lockId, [FromBody] UpdateLock.Request newLock)
        {
            newLock.LockId = lockId;
            return await _sender.Send(newLock);
        }       

        [HttpDelete("{lockId}")]
        public async Task<DeleteLock.Response> DeleteLock(Guid lockId) =>
               await _sender.Send(new DeleteLock.Request { LockId = lockId });

        //Allow User To Lock
        [HttpPost("{lockId}/assign")]
        public async Task<AssignAccessToLock.Response> AssignAccessToLock(Guid lockId, [FromBody] AllowedUser allowedUser) =>
               await _sender.Send(new AssignAccessToLock.Request { LockId = lockId, CardholderId = allowedUser.CardholderId, ScheduleId = allowedUser.ScheduleId });

        [HttpPut("{lockId}/edit/{cardholderId}")]
        public async Task<EditAccessToLock.Response> EditAccessToLock(Guid lockId, Guid cardholderId, [FromBody] AllowedUser allowedUser) =>
               await _sender.Send(new EditAccessToLock.Request {LockId = lockId, CardholderId = cardholderId, ScheduleId = allowedUser.ScheduleId });

        [HttpDelete("{lockId}/remove/{cardholderId}")]
        public async Task<RemoveAccessFromLock.Response> RemoveAccessFromLock(Guid siteId, Guid lockId, Guid cardholderId) =>
               await _sender.Send(new RemoveAccessFromLock.Request { SiteId = siteId, LockId = lockId, CardholderId = cardholderId });

        //Lock/Unlock Door Triggers
        [HttpPut("{lockId}/lock")]
        public async Task<TriggerLockDoor.Response> TriggerLockDoor(Guid lockId, [FromBody] TriggerLockDoor.Request request)
        {
            request.LockId = lockId;
            return await _sender.Send(request);
        }

        [HttpPut("{lockId}/unlock")]
        public async Task<TriggerUnlockDoor.Response> TriggerUnlockDoor(Guid lockId, [FromBody] TriggerUnlockDoor.Request request)
        {
            request.LockId = lockId;
            return await _sender.Send(request);
        }
    }
}