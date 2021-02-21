using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Group: AbstractUser
    {
        [MinLength(3)]
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
