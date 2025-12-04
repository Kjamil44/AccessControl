using Marten.Schema;

namespace AccessControl.API.Models
{
    public class UserRoleAssignment
    {
        [Identity]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
