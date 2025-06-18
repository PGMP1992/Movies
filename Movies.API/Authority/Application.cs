namespace Movies.API.Authority
{
    public class Application
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ClientId { get; set; }
        public string? Secret { get; set; }
        public string? Scopes { get; set; }
    }
}
