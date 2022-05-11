using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace PersonManagement.MVC
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var pathValue = context.Request.Path.Value.ToLower();

            if (pathValue.Contains("guest") || pathValue.Contains("login") || pathValue.Contains("register"))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Cookies.FirstOrDefault(x => x.Key == Constants.Token);
            if (token.Key == null || !Token.ValidateToken(token.Value))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Redirect("/authentication/login");
                return;
            }

            await _next(context);
        }
    }
}
