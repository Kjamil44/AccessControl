namespace AccessControl.API.Exceptions
{
    [Serializable]
    public class CoreException : Exception
    {
        public CoreError CoreError { get; private set; }

        public CoreException(string errorMessage, int statusCode = 400) : base(errorMessage)
        {
            CoreError = new CoreError(errorMessage, statusCode);
        }

        public CoreException(string errorMessage, IEnumerable<CoreErrorItem> errors, int statusCode = 400) : this(errorMessage, statusCode)
        {
            if (errors != null)
                CoreError.Errors = errors;
        }
    }
}
