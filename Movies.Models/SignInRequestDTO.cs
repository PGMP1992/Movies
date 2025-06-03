using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class SignInRequestDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        //[RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string UserName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
