using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class PlaylistDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string AppUserId { get; set; }
        
        public AppUserDto? AppUser { get; set; }
    }
}
