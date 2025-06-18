using Microsoft.AspNetCore.Mvc;
using Movies.API.Authority;

namespace Movies.API.Controllers
{
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthorityController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            if (credential == null || string.IsNullOrEmpty(credential.ClientId) || string.IsNullOrEmpty(credential.Secret))
            {
                return BadRequest("Invalid client credentials.");
            }

            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, _config["SecurityKey"] ?? string.Empty),
                    expires_At = expiresAt,
                });
            }
            else
            {
                ModelState.AddModelError("Unauthorised", "You are not authorized!");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Authentication Failed",
                    Detail = "The provided client credentials are invalid."
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
        }
        }
}
