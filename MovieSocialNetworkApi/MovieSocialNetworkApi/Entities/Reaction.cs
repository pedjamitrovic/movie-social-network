using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Reaction
    {
        [Key]
        public int Id { get; set; }
        public int Value { get; set; }
        public virtual SystemEntity Owner { get; set; }
        public virtual Content Content { get; set; }
    }
}
