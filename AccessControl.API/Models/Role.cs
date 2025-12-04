using Marten.Schema;

namespace AccessControl.API.Models
{
    public class Role
    {
        [Identity]
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public List<string> Permissions { get; set; } = [];
        public bool IsSystemRole { get; set; }
    }
}
