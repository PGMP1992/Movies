using Newtonsoft.Json;

namespace MoviesApp.Services
{
    public class JwtToken
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        
        [JsonProperty("expiresAt")]
        public DateTime ExpiresAt { get; set; }
    }
}
