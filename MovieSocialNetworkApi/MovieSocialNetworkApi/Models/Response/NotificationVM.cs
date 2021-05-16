using System;

namespace MovieSocialNetworkApi.Models
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool Seen { get; set; }
        public byte[] Extended { get; set; }
        public virtual SystemEntityVM Sender { get; set; }
        public virtual SystemEntityVM Recepient { get; set; }
    }
}