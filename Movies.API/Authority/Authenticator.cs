using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Movies.API.Authority
{
    public class Authenticator
    {
        public static bool Authenticate(string clientId, string secret, IConfiguration _config)
        {
            var configClientId = _config.GetValue<string>("ClientId") ?? throw new ArgumentNullException("ClientId is not configured.");
            var configSecret = _config.GetValue<string>("Secret") ?? throw new ArgumentNullException("ClientId is not configured.");
            return (configClientId == clientId && configSecret == secret);
        }

        public static string CreateToken(string clientId, DateTime expiresAt, string strSecretKey, IConfiguration _config)
        {
            // Algorithm to create a token
            //Signing the token with a secret key
            //Payload can include claims, expiration, etc.

            // Augorithm 
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(strSecretKey))
                , SecurityAlgorithms.HmacSha256Signature);

            //Payload( claims, expiration, etc.)
            //var configClientId = _config.GetValue<string>("ClientId") ?? throw new ArgumentNullException("ClientId is not configured.");
            //var configSecret = _config.GetValue<string>("Secret") ?? throw new ArgumentNullException("ClientId is not configured.");
            var configScopes = _config.GetValue<string>("Scopes") ?? throw new ArgumentNullException("Scopes is not configured.");

            var claimsDictionary = new Dictionary<string, object>
            {
                { "Read", (configScopes ?? string.Empty ).Contains("read") ? "true" : "false"},
                { "Write", (configScopes ?? string.Empty).Contains("write") ? "true" : "false" },
                { "Delete", (configScopes ?? string.Empty).Contains("delete") ? "true" : "false" }
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = signingCredentials,
                Claims = claimsDictionary,
                Expires = expiresAt,
                NotBefore = DateTime.UtcNow,
            };

            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public static async Task<bool> VerifyTokenAsync(string token, string? securityKey)
        {
            if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(securityKey))
            {
                return false;
            }
            try
            {
                var keyBytes = System.Text.Encoding.UTF8.GetBytes(securityKey);
                var tokenHandler = new JsonWebTokenHandler();
                
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero 
                };

                var result = await tokenHandler.ValidateTokenAsync(token, validationParameters);
                return result.IsValid;
            }
            catch(SecurityTokenExpiredException)
            {
                return false;
            }
            catch(SecurityTokenInvalidSignatureException)
            {
                return false;
            }
            catch(SecurityTokenMalformedException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
