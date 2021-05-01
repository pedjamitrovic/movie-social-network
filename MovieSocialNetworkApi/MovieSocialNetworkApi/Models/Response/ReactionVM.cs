using static MovieSocialNetworkApi.Helpers.Enumerations;

namespace MovieSocialNetworkApi.Models
{
    public class ReactionVM
    {
        public int Id { get; set; }
        public ReactionType Value { get; set; }
    }
}
