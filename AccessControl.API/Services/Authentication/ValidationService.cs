using AccessControl.API.Models;
using JasperFx.Core;
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
            //TODO: Set a condition where there is no User table into the db (the db is empty)
            var userExists = await session.Query<User>().AnyAsync(x => x.Email.EqualsIgnoreCase(email));
            if (userExists)
            {
                throw new Exception("User with that email already exists");
            }
        }
    }
}
