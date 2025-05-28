namespace Movies.Models
{
    public class ErrorModelDto
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
