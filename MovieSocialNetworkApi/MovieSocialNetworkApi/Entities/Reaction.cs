using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Reaction
    {
        [Key]
        public long Id { get; set; }
        public long Value { get; set; }
        public virtual AbstractUser Owner { get; set; }
        public virtual AbstractContent Content { get; set; }
    }
}
