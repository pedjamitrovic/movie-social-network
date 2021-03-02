using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class RegisterCommand
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
