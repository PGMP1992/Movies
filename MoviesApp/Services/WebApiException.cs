using System.Text.Json;

namespace MoviesApp.Services
{
    public class WebApiException : Exception
    {
        public ErrorResponse? ErrorResponse { get; }
        
        public WebApiException(string errorJson) 
        {
            ErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorJson);
        }
    }
}
