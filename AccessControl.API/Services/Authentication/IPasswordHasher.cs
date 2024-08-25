namespace AccessControl.API.Services.Authentication
{
    public interface IPasswordHasher
    {
        string HashPassword(string passwordPlain);
        bool VerifyHashedPassword(string passwordHash, string passwordPlain);
    }
}
