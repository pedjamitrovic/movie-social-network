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
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        // profile image, cover image
    }
}
