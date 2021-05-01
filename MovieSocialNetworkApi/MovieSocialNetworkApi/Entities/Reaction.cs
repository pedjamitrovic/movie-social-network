using System.ComponentModel.DataAnnotations;
using static MovieSocialNetworkApi.Helpers.Enumerations;

namespace MovieSocialNetworkApi.Entities
{
    public class Reaction
    {
        [Key]
        public int Id { get; set; }
        public ReactionType Value { get; set; }
        public virtual SystemEntity Owner { get; set; }
        public virtual Content Content { get; set; }
    }
}
