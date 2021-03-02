using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class LoginCommand
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
