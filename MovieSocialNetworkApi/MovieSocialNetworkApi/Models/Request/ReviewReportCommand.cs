using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MovieSocialNetworkApi.Models
{
    public class ReviewReportCommand
    {
        [Required]
        public bool IssueBan { get; set; }
        public DateTimeOffset BannedUntil { get; set; }
        public string Reason { get; set; }
    }
}
