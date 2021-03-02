using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Relation
    {
        public long FollowerId { get; set; }
        public long FollowingId { get; set; }

        public virtual AbstractUser Follower { get; set; }
        public virtual AbstractUser Following { get; set; }
    }
}
