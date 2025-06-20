using System.Text.Json.Serialization;

namespace Movies.DataAccess.Models
{
    public class ErrorModelDto
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
