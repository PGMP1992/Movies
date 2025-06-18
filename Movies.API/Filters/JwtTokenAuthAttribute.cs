using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.API.Authority;

namespace Movies.API.Filters
{
    public class JwtTokenAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if( ! context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            string token = authorizationHeader.ToString().Replace("Bearer ", string.Empty);

            // Get the configuration and SecretKey from your configuration
            var securityKey = context.HttpContext.RequestServices.GetService<IConfiguration>()?.GetValue<string>("SecurityKey");

            if( await Authenticator.VerifyTokenAsync(token, securityKey))
            {
                return;
            }
            else { 
                context.Result = new UnauthorizedResult();
                return;
            }
        }

    }
}
