using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Relation
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }

        public virtual SystemEntity Follower { get; set; }
        public virtual SystemEntity Following { get; set; }
    }
}
