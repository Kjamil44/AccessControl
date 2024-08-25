using AccessControl.API.Models;

namespace AccessControl.API.Services.Authentication.JwtFeatures
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(User user);
    }
}
