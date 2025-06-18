using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Movies.API.Authority
{
    public static class Authenticator
    {
        public static bool Authenticate(string clientId, string secret)
        {
            var app = AppRepos.GetByClientId(clientId);
            if (app == null)
                return false;

            return (app.ClientId == clientId && app.Secret == secret);
        }


        public static string CreateToken(string clientId, DateTime expiresAt, string strSecretKey)
        {
            // Algorithm to create a token
            //Signing the token with a secret key
            //Payload can include claims, expiration, etc.

            // Augorithm 
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(strSecretKey))
                , SecurityAlgorithms.HmacSha256Signature);

            //Payload( claims, expiration, etc.)
            var app = AppRepos.GetByClientId(clientId);
            var claimsDictionary = new Dictionary<string, object>
            {
                { "Name", app?.Name ?? string.Empty },
                { "Read", (app?.Scopes ?? string.Empty ).Contains("read") ? "true" : "false"},
                { "Write", (app?.Scopes ?? string.Empty).Contains("write") ? "true" : "false" }
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
