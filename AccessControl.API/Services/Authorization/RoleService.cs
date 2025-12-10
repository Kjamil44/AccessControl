using AccessControl.API.Authorization;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using Marten;

namespace AccessControl.API.Services.Authorization
{
    public class RoleService : IRoleService
    {
        private readonly IDocumentSession _session;

        public RoleService(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<Role?> GetRoleAsync(Guid id)
            => await _session.LoadAsync<Role>(id);

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
            => await _session.Query<Role>().ToListAsync();

        public async Task<Role> CreateRoleAsync(string name, List<string> permissions)
        {
            var isValid = permissions.Any(p => PermissionCatalog.IsValid(p));
            if (!isValid)
                throw new CoreException("Invalid permission found.");

            var role = new Role
            {
                Name = name,
                Permissions = permissions
            };

            _session.Store(role);
            await _session.SaveChangesAsync();

            return role;
        }

        public async Task UpdateRolePermissionsAsync(Guid roleId, List<string> permissions)
        {
            var role = await _session.LoadAsync<Role>(roleId)
                ?? throw new CoreException("Role not found.");

            if (role.IsSystemRole)
                throw new InvalidOperationException("System roles cannot be modified.");

            if (permissions.Any(p => !PermissionCatalog.IsValid(p)))
                throw new ArgumentException("Invalid permission found.");

            role.Permissions = permissions;

            _session.Store(role);
            await _session.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            var role = await _session.LoadAsync<Role>(roleId)
                ?? throw new CoreException("Role not found.");

            if (role.IsSystemRole)
                throw new InvalidOperationException("System roles cannot be deleted.");

            _session.Delete(role);
            await _session.SaveChangesAsync();
        }
    }
}
