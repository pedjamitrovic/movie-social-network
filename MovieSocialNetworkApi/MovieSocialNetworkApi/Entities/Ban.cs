using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Ban
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset BannedFrom { get; set; }
        public DateTimeOffset BannedUntil { get; set; }
        public string Reason { get; set; }
        public virtual SystemEntity BannedEntity { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
