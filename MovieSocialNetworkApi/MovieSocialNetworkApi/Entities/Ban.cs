using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Ban
    {
        [Key]
        public int Id { get; set; }
        public DateTime BannedFrom { get; set; }
        public DateTime BannedUntil { get; set; }
        public string Reason { get; set; }
        public virtual SystemEntity BannedEntity { get; set; }
    }
}
