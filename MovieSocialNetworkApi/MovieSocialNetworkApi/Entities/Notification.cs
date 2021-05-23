using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public int SenderId { get; set; }
        public int RecepientId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool Seen { get; set; }
        public string Extended { get; set; }
        public virtual SystemEntity Sender { get; set; }
        public virtual SystemEntity Recepient { get; set; }
    }
}
