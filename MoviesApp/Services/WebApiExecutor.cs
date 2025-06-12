using MoviesApp.Services.Interfaces;
using System.Text.Json;

namespace MoviesApp.Services
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "MoviesApi";
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
            var response = await client.PostAsJsonAsync<T>(relativeUrl, obj);
            await HandlePottentiaError(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var client = _httpClientFactory.CreateClient(apiName);
            var response = await client.PutAsJsonAsync<T>(relativeUrl, obj);
            await HandlePottentiaError(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var client = _httpClientFactory.CreateClient(apiName);
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
    }
}
