namespace AccessControl.API.Exceptions
{
    public class CoreError
    {
        public string ErrorMessage { get; set; }
        public IEnumerable<CoreErrorItem> Errors { get; set; } = Enumerable.Empty<CoreErrorItem>();
        public string ErrorCode { get; protected set; } = Guid.NewGuid().ToString();

        public CoreError()
        {
        }

        public CoreError(int statusCode)
        {
            _statusCode = statusCode;
        }

        public CoreError(string errorMessage, int statusCode)
        {
            ErrorMessage = errorMessage;
            _statusCode = statusCode;
        }

        private int _statusCode = 400;
        public int GetStatusCode() => _statusCode;

        public static CoreError CreateUnhandled()
        {
            var coreError = new CoreError(500);
            coreError.ErrorMessage = $"An error occurred. Please contact your system administrator with the following error code: ({coreError.ErrorCode}).";
            return coreError;
        }

        public static CoreError CreateUnauthorized()
        {
            return new CoreError("Unauthorized access.", 401);
        }
    }

    public class CoreErrorItem
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public object AttemptedValue { get; set; }
    }
}
