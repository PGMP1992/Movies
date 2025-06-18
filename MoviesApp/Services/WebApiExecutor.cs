using MoviesApp.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MoviesApp.Services
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "MoviesApi";
        private const string authApi = "AuthApi";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IConfiguration _config;
        private readonly string _baseServerUrl;

        public WebApiExecutor(IHttpClientFactory httpClientFactory
            ,IConfiguration config
            ,IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            //_baseServerUrl = _configuration.GetValue<string>("BaseServerUrl") ?? throw new ArgumentNullException("BaseServerUrl is not configured.");
        }
        
        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var client = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(client);
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Activator.CreateInstance<T>();
            }
            await HandlePottentiaError(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            var client = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(client);
            var response = await client.PostAsJsonAsync<T>(relativeUrl, obj);
            await HandlePottentiaError(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var client = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(client);
            var response = await client.PutAsJsonAsync<T>(relativeUrl, obj);
            await HandlePottentiaError(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var client = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(client);
            var response = await client.DeleteAsync(relativeUrl);
            await HandlePottentiaError(response);
        }

        private async Task HandlePottentiaError(HttpResponseMessage response)
        {
            if (! response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                throw new WebApiException(errorJson);
            }
        }

        private async Task AddJwtToHeader( HttpClient client)
        {
            JwtToken? token = null;
            string? strToken = _httpContextAccessor.HttpContext?.Session.GetString("access_token");
            if (! string.IsNullOrWhiteSpace(strToken))
            { 
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);
            }
            
            // Create or Renew Token after 10 minutes 
            if(token == null || token.ExpiresAt <= DateTime.UtcNow)
            {
                var clientId = _config.GetValue<string>("ClientId") ?? throw new ArgumentNullException("ClientId is not configured.");
                var secret = _config.GetValue<string>("Secret") ?? throw new ArgumentNullException("Secret is not configured.");

                // Authenticate
                var authClient = _httpClientFactory.CreateClient(authApi);
                var response = await authClient.PostAsJsonAsync("auth", new AppCredential
                {
                    ClientId = clientId,
                    Secret = secret,
                });
                response.EnsureSuccessStatusCode();

                // Read the token from the response
                strToken = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<JwtToken>(strToken) ?? throw new InvalidOperationException("Token is null or empty.");
                _httpContextAccessor.HttpContext?.Session.SetString("access_token", strToken);
            }

            //Pass the token to the client header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }
    }
}
