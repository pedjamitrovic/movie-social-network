using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Comment: AbstractContent
    {
        public Post Post { get; set; }
    }
}
