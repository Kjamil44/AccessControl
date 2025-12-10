using AccessControl.API.Models;

namespace AccessControl.API.Services.Authorization
{
    public interface IRoleService
    {
        Task<Role?> GetRoleAsync(Guid id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> CreateRoleAsync(string name, List<string> permissions);
        Task UpdateRolePermissionsAsync(Guid roleId, List<string> permissions);
        Task DeleteRoleAsync(Guid roleId);
    }
}
