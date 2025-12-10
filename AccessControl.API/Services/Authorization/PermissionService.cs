using AccessControl.API.Models;
using Marten;

namespace AccessControl.API.Services.Authorization
{
    public class PermissionService : IPermissionService
    {
        private readonly IDocumentSession _session;

        public PermissionService(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId)
        {
            var assignments = await _session
                .Query<UserRoleAssignment>()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var roleIds = assignments.Select(a => a.RoleId).Distinct().ToList();

            var roles = await _session
                .Query<Role>()
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync();

            return roles
                .SelectMany(r => r.Permissions)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permission)
        {
            var permissions = await GetPermissionsForUserAsync(userId);
            return permissions.Contains(permission);
        }

        public async Task<bool> HasAnyPermissionAsync(Guid userId, IEnumerable<string> permissions)
        {
            var userPerms = await GetPermissionsForUserAsync(userId);
            return permissions.Any(p => userPerms.Contains(p));
        }
    }
}
