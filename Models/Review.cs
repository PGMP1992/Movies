using System.Net.NetworkInformation;

namespace MoviesApp.Models
{
    public class Review : EntityBase
    {
        public string? MovieId { get; set; }
        public string? ReviewText { get; set; }
        // Navigation properties
        public virtual AppUser? AppUser { get; set; }
        public virtual Movie? Movie { get; set; }
        public IEnumerable<Reply>? Replies { get; set; } = new List<Reply>();
    }
}
