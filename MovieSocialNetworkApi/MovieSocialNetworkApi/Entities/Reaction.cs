using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Reaction
    {
        [Key]
        public long Id { get; set; }
        public long Value { get; set; }
        public AbstractUser Owner { get; set; }
        public AbstractContent Content { get; set; }
    }
}
