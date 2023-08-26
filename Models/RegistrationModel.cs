using System.ComponentModel.DataAnnotations;

namespace DotNetAPI.Models
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)] // You can add more validation rules as needed
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation do not match.")]
        public string PasswordConfirmation { get; set; } = string.Empty;
    }

}
