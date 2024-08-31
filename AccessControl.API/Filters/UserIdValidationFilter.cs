using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace AccessControl.API.Filters
{
    public class UserIdValidationFilter : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdValidationFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = GetUserIdFromToken();
            if (userId != null)
            {
                // Add the userId to action arguments or use it as needed
                context.ActionArguments["userId"] = Guid.Parse(userId);
            }

            await next();
        }

        private string GetUserIdFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                return null;
            }

            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var userId = jwtToken?.Claims.First(claim => claim.Type == "sub")?.Value;

            return userId;
        }
    }
}
