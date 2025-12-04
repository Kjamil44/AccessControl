using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Authentication;
using AccessControl.API.Services.Authentication.JwtFeatures;
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

            public Handler(
                IDocumentSession session,
                IPasswordHasher passwordHasher,
                IJwtTokenGenerator jwtTokenGenerator)
            {
                _session = session;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<Response> Handle(Request req, CancellationToken ct)
            {
                var email = req.Email.Trim().ToLowerInvariant();

                var exists = await _session.Query<User>()
                    .AnyAsync(x => x.Email.EqualsIgnoreCase(email));

                if (exists)
                    throw new CoreException("A user with this email already exists.");

                var user = new User
                {
                    Username = req.Username,
                    Email = email,
                    PasswordHash = _passwordHasher.HashPassword(req.Password),
                };

                _session.Store(user);

                var token = _jwtTokenGenerator.GenerateToken(user);

                return new Response(token);
            }
        }
    }
}
