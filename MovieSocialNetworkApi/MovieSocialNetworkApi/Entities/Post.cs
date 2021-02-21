using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Post: AbstractContent
    {
        public ICollection<Comment> Comments { get; set; }
        public Group OwnerGroup { get; set; }
        // Media
    }
}
