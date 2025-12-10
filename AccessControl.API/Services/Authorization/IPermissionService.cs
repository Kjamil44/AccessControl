namespace AccessControl.API.Services.Authorization
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId);
        Task<bool> HasPermissionAsync(Guid userId, string permission);
        Task<bool> HasAnyPermissionAsync(Guid userId, IEnumerable<string> permissions);
    }
}
