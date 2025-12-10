using AccessControl.API.Models;

namespace AccessControl.API.Services.Authorization
{
    public interface IUserRoleService
    {
        Task AssignRoleAsync(Guid userId, Guid roleId);
        Task<IEnumerable<UserRoleAssignment>> GetAssignmentsAsync(Guid userId);
        Task RemoveRoleAsync(Guid userId, Guid roleId);
    }
}
