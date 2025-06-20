using Movies.DataAccess.Models;
using System.Text.Json;

namespace MoviesApp.Services
{
    public class WebApiException : Exception
    {
        public ErrorModelDto? Response { get; }

        public WebApiException(string errorJson)
        {
            try
            {
                Response = JsonSerializer.Deserialize<ErrorModelDto>(errorJson);
            }
            catch (Exception)
            {
                throw;
            }
        }    
    }
}
