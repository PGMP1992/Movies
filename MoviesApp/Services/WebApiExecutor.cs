using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "API";
        private readonly IHttpClientFactory _httpClientFactory;

        //private readonly IConfiguration _configuration;
        //private readonly string _baseServerUrl;

        public WebApiExecutor(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            //_configuration = configuration;
            //_baseServerUrl = _configuration.GetValue<string>("BaseServerUrl") ?? throw new ArgumentNullException("BaseServerUrl is not configured.");
        }
        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var client = _httpClientFactory.CreateClient(apiName);
            var result =  await client.GetFromJsonAsync<T>(relativeUrl);
            return result;
        }
    }
}
