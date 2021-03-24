using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class GroupAdmin
    {
        public int AdminId { get; set; }
        public int GroupId { get; set; }

        public virtual User Admin { get; set; }
        public virtual Group Group { get; set; }
    }
}
