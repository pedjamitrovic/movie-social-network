using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class CreateReactionCommand
    {
        [Required]
        public int Value { get; set; }
    }
}
