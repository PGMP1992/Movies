namespace MoviesApp.DTOs
{
    public class ErrorModelDTO
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
