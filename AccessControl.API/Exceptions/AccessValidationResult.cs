namespace AccessControl.API.Exceptions
{
    public record AccessValidationResult(bool IsAllowed, string? Reason = null);
}
