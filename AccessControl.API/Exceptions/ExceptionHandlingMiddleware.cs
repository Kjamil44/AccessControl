namespace AccessControl.API.Exceptions
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CoreException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.CoreError.GetStatusCode();
                await context.Response.WriteAsJsonAsync(ex.CoreError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                var coreError = CoreError.CreateUnhandled();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = coreError.GetStatusCode();
                await context.Response.WriteAsJsonAsync(coreError);
            }
        }
    }

}
