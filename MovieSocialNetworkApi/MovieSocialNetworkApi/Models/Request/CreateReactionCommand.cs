using System.ComponentModel.DataAnnotations;
using static MovieSocialNetworkApi.Helpers.Enumerations;

namespace MovieSocialNetworkApi.Models
{
    public class CreateReactionCommand
    {
        [Required]
        public ReactionType Value { get; set; }
    }
}
