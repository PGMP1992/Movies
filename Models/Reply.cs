namespace MoviesApp.Models
{
    public class Reply
    {
        public string? ReviewId { get; set; }
        public string? ReplyText { get; set; }
        // Navigation properties
        public virtual AppUser? AppUser { get; set; }
        public virtual Review? Review { get; set; }
    }
}