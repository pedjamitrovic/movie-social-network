using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Post: AbstractContent
    {
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Group OwnerGroup { get; set; }
        // Media
    }
}
