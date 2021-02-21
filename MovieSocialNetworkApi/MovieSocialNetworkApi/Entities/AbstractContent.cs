using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public abstract class AbstractContent
    {
        [Key]
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
        public AbstractUser Creator { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
    }
}
