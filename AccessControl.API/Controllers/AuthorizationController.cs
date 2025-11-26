using AccessControl.API.Handlers.AuthorizationHandlers;
using AccessControl.API.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [ApiController]
    [Route("auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender) => _sender = sender;

        [HttpPost("register")]
        public async Task<ActionResult<RegisterUser.Response>> Register([FromBody] RegisterUserDto dto)
        {
            var response = await _sender.Send(new RegisterUser.Request(dto.Username, dto.Email, dto.Password));

            return Ok(response);
        }

        [HttpPost("login")]
        public Task<LoginUser.Response> Login([FromBody] LoginUserDto dto)
            => _sender.Send(new LoginUser.Request(dto.Email, dto.Password));
    }
}
