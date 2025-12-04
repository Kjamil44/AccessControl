using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Authentication;
using AccessControl.API.Services.Authentication.JwtFeatures;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.AuthenticationHandlers
{
    public class LoginUser
    {
        public sealed record Request(string Email, string Password)
            : IQuery<Response>;

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

                var user = await _session.Query<User>()
                                       .SingleOrDefaultAsync(x => x.Email == email);

                if (user is null || !_passwordHasher.VerifyHashedPassword(user.PasswordHash, req.Password))
                    throw new CoreException("Invalid email or password.", statusCode: 401);

                var token = _jwtTokenGenerator.GenerateToken(user);

                return new Response(token);
            }
        }
    }
}
