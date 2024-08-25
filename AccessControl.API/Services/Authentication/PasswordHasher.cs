namespace AccessControl.API.Services.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string passwordPlain)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordPlain);
            return hashedPassword;
        }

        public bool VerifyHashedPassword(string passwordHash, string passwordPlain)
        {
            return BCrypt.Net.BCrypt.Verify(passwordPlain, passwordHash);
        }
    }
}
