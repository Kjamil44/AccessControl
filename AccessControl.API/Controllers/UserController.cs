
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
    }
}
