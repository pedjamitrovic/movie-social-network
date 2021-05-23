using System;

namespace MovieSocialNetworkApi.Models
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool Seen { get; set; }
        public string Extended { get; set; }
        public SystemEntityVM Sender { get; set; }
    }
}