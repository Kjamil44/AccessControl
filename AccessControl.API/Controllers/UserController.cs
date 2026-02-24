
using AccessControl.API.Handlers.UserHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender) => _sender = sender;

        [HttpGet("current")]
        public async Task<GetCurrentUser.Response> GetCurrentUser(Guid userId) => await _sender.Send(new GetCurrentUser.Request { UserId = userId });

        [HttpPut("{targetUserId}/role")]
        public async Task<ChangeUserRole.Response> ChangeUserRole(Guid targetUserId, Guid userId, [FromBody] ChangeUserRoleDto dto)
        {
            return await _sender.Send(new ChangeUserRole.Request
            {
                TargetUserId = targetUserId,
                RoleName = dto.RoleName,
                RequestingUserId = userId
            });
        }
    }

    public class ChangeUserRoleDto
    {
        public string RoleName { get; set; }
    }
}
