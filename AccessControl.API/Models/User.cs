using Marten.Schema;

namespace AccessControl.API.Models
{
    public class User
    {
        [Identity]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
