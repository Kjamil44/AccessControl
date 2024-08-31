using AccessControl.API.Models;
using Baseline;
using Marten;

namespace AccessControl.API.Services.Authentication
{
    public class ValidationService : IValidationService
    {
        public ValidationService()
        {
        }

        public async Task CheckIfUserAlreadyExists(IDocumentSession session, string email)
        {
            var userExists = await session.Query<User>().AnyAsync(x => x.Email.EqualsIgnoreCase(email));
            if (userExists)
            {
                throw new Exception("User with that email already exists");
            }
        }
    }
}
