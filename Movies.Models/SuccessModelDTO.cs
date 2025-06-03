namespace Movies.Models
{
    public class SuccessModelDTO
    {
        public int StatusCode { get; set; }
        public string SuccessMessage { get; set; } = string.Empty;
        public object Data { get; set; }
    }
}
