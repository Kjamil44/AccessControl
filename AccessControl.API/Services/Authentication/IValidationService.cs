using Marten;

namespace AccessControl.API.Services.Authentication
{
    public interface IValidationService
    {
        public Task CheckIfUserAlreadyExists(IDocumentSession session, string email);
    }
}
