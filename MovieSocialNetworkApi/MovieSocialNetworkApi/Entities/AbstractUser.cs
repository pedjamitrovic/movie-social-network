using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public abstract class AbstractUser
    {
        [Key]
        public long Id { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public DateTime BannedUntil { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Relation> Following { get; set; }
        public virtual ICollection<Relation> Followers { get; set; }
        // profile image, cover image
    }
}
