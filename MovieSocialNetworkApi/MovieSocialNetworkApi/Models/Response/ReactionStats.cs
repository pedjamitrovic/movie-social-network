using static MovieSocialNetworkApi.Helpers.Enumerations;

namespace MovieSocialNetworkApi.Models.Response
{
    public class ReactionStats
    {
        public ReactionType Value { get; set; }
        public int Count { get; set; }
    }
}
