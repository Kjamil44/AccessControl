using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Authorization;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.UserHandlers
{
    public class GetCurrentUser
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
        }
        public class Response
        {
            public string Email { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
            public List<string> Permissions { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            private readonly IUserRoleService _userRoleService;
            private readonly IRoleService _roleService;

            public Handler(IDocumentSession session, IUserRoleService userRoleService, IRoleService roleService)
            {
                _session = session;
                _userRoleService = userRoleService;
                _roleService = roleService;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _session.LoadAsync<User>(request.UserId);
                if (user == null)
                    throw new CoreException("User not found");

                var userRoleAssignment = await _userRoleService.GetAssignmentAsync(user.Id);
                if (userRoleAssignment == null)
                    throw new CoreException("User is not assigned to a specific Role");

                var role = await _roleService.GetRoleAsync(userRoleAssignment.RoleId);
                if (role == null)
                    throw new CoreException("Role not found");

                return new Response
                {
                    Email = user.Email,
                    Username = user.Username,
                    Role = role.Name,
                    Permissions = role.Permissions,
                };
            }
        }
    }
}
