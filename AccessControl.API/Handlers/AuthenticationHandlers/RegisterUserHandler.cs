using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Authentication;
using AccessControl.API.Services.Authentication.JwtFeatures;
using AccessControl.API.Services.Authorization;
using AccessControl.API.Authorization;
using JasperFx.Core;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.AuthenticationHandlers
{
    public class RegisterUser
    {
        public sealed record Request(string Username, string Email, string Password)
         : ICommand<Response>;

        public sealed record Response(string Token);

        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IRoleService _roleService;
            private readonly IUserRoleService _userRoleService;

            public Handler(
                IDocumentSession session,
                IPasswordHasher passwordHasher,
                IJwtTokenGenerator jwtTokenGenerator,
                IRoleService roleService,
                IUserRoleService userRoleService)
            {
                _session = session;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _roleService = roleService;
                _userRoleService = userRoleService;
            }

            public async Task<Response> Handle(Request req, CancellationToken ct)
            {
                var email = req.Email.Trim().ToLowerInvariant();

                var exists = await _session.Query<User>()
                    .AnyAsync(x => x.Email.EqualsIgnoreCase(email));

                if (exists)
                    throw new CoreException("A user with this email already exists.");

                var role = await _roleService.GetRoleByNameAsync(RoleNames.Auditor);
                if (role == null)
                    throw new CoreException("Default role not found. Please seed the database.");

                var user = new User
                {
                    Username = req.Username,
                    Email = email,
                    PasswordHash = _passwordHasher.HashPassword(req.Password),
                };

                _session.Store(user);

                _userRoleService.AssignRoleAsync(user.Id, role.Id);

                var token = _jwtTokenGenerator.GenerateToken(user);

                return new Response(token);
            }
        }
    }
}
