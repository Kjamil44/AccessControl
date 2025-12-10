using Marten.Schema;

namespace AccessControl.API.Models
{
    public class UserRoleAssignment
    {
        [Identity]
        public Guid Id { get; set; }
        [ForeignKey(typeof(User))]
        public Guid UserId { get; set; }
        [ForeignKey(typeof(Role))]
        public Guid RoleId { get; set; }
    }
}
