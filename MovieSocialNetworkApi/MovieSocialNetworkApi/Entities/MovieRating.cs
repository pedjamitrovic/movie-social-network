using System.ComponentModel.DataAnnotations;
using static MovieSocialNetworkApi.Helpers.Enumerations;

namespace MovieSocialNetworkApi.Entities
{
    public class MovieRating
    {
        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int? Rating { get; set; }
        public virtual SystemEntity Owner { get; set; }
    }
}
