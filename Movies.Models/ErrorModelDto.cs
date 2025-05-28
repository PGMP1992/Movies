namespace Movies.Models
{
    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
