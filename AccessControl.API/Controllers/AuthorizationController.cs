using AccessControl.API.Models;
using AccessControl.API.Models.Entities;
using AccessControl.API.Services.Authentication;
using AccessControl.API.Services.Authentication.JwtFeatures;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [ApiController]
    [Route("auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IDocumentSession _documentStore;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidationService _validationService;

        public AuthController(IDocumentSession documentStore, IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher, IValidationService validationService)
        {
            _documentStore = documentStore;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
            _validationService = validationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var user = new User
            {
                Username = registerUserDto.Username,
                Email = registerUserDto.Email.Trim().ToLower(),
                PasswordHash = _passwordHasher.HashPassword(registerUserDto.Password),
            };

            await _validationService.CheckIfUserAlreadyExists(_documentStore, user.Email);

            _documentStore.Store(user);

            await _documentStore.SaveChangesAsync();

            var token = _jwtTokenGenerator.GenerateToken(user);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var user = await _documentStore.Query<User>().SingleOrDefaultAsync(x => x.Email == loginUserDto.Email);

            if (user == null || !_passwordHasher.VerifyHashedPassword(user.PasswordHash, loginUserDto.Password))
            {
                return Unauthorized();
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
