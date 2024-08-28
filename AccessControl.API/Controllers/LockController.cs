using AccessControl.API.Handlers.LockHandlers;
using AccessControl.API.Handlers.AllowedUserHandlers;
using AccessControl.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AccessControl.API.Controllers
{
    [Route("api/locks")]
    [Authorize]
    [ApiController]
    public class LockController : ControllerBase
    {
        private readonly ISender _sender;
        public LockController(ISender sender) => _sender = sender;

        [HttpGet("site/{siteId}")]
        public async Task<GetLocks.Response> GetLocks([FromRoute] Guid siteId) => await _sender.Send(new GetLocks.Request { SiteId = siteId });

        [HttpGet("{lockId}")]
        public async Task<GetLock.Response> GetLock(Guid lockId) => await _sender.Send(new GetLock.Request { LockId = lockId });

        [HttpPost("create")]
        public async Task<AddLock.Response> CreateLock( [FromBody] AddLock.Request newLock) => await _sender.Send(newLock);

        [HttpPut("update/{lockId}")]
        public async Task<UpdateLock.Response> UpdateLock(Guid lockId, [FromBody] UpdateLock.Request newLock)
        {
            newLock.LockId = lockId;
            return await _sender.Send(newLock);
        }       

        [HttpDelete("delete/{lockId}")]
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
    }
}