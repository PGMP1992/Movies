using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class PlaylistDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string AppUserId { get; set; } = string.Empty;

        public AppUserDto? AppUser { get; set; }
    }
}
