namespace Movies.Models
{
    public class AppUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string? ProfileImageryUrl { get; set; } = string.Empty;
    }
}
