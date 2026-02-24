using AccessControl.API.Authorization;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Authorization;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.UserHandlers
{
    public class ChangeUserRole
    {
        public class Request : ICommand<Response>
        {
            public Guid TargetUserId { get; set; }
            public string RoleName { get; set; }
            public Guid RequestingUserId { get; set; }
        }

        public class Response
        {
            public Guid UserId { get; set; }
            public string RoleName { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            private readonly IRoleService _roleService;
            private readonly IUserRoleService _userRoleService;
            private readonly IPermissionService _permissionService;

            public Handler(
                IDocumentSession session,
                IRoleService roleService,
                IUserRoleService userRoleService,
                IPermissionService permissionService)
            {
                _session = session;
                _roleService = roleService;
                _userRoleService = userRoleService;
                _permissionService = permissionService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var hasPermission = await _permissionService.HasPermissionAsync(
                    request.RequestingUserId, PermissionCatalog.User.Update);

                if (!hasPermission)
                    throw new CoreException("You do not have permission to change user roles.", 403);

                var targetUser = await _session.LoadAsync<User>(request.TargetUserId);
                if (targetUser == null)
                    throw new CoreException("User not found.");

                var newRole = await _roleService.GetRoleByNameAsync(request.RoleName);
                if (newRole == null)
                    throw new CoreException("Role not found.");

                // Remove existing role assignments
                var existingAssignments = await _userRoleService.GetAssignmentsAsync(request.TargetUserId);
                foreach (var assignment in existingAssignments)
                {
                    await _userRoleService.RemoveRoleAsync(request.TargetUserId, assignment.RoleId);
                }

                // Assign the new role
                _userRoleService.AssignRoleAsync(request.TargetUserId, newRole.Id);

                return new Response
                {
                    UserId = request.TargetUserId,
                    RoleName = newRole.Name
                };
            }
        }
    }
}
