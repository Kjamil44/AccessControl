using AccessControl.API.Models;
using Marten;

namespace AccessControl.API.Services.Authorization
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IDocumentSession _session;

        public UserRoleService(IDocumentSession session)
        {
            _session = session;
        }

        public async Task AssignRoleAsync(Guid userId, Guid roleId)
        {
            var assignment = new UserRoleAssignment
            {
                UserId = userId,
                RoleId = roleId
            };

            _session.Store(assignment);
            await _session.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserRoleAssignment>> GetAssignmentsAsync(Guid userId)
            => await _session.Query<UserRoleAssignment>()
                       .Where(x => x.UserId == userId)
                       .ToListAsync();

        public async Task RemoveRoleAsync(Guid userId, Guid roleId)
        {
            var assignments = await GetAssignmentsAsync(userId);

            var target = assignments.FirstOrDefault(a => a.RoleId == roleId);
            if (target == null)
                return;

            _session.Delete(target);
            await _session.SaveChangesAsync();
        }
    }
}
