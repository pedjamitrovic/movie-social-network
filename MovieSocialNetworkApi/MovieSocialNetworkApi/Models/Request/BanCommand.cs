using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MovieSocialNetworkApi.Models
{
    public class BanCommand
    {
        [Required]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [Required]
        [DataMember(Name = "bannedUntil")]
        public DateTime BannedUntil { get; set; }
    }
}
