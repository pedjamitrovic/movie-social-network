using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ChatRoomId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool Seen { get; set; }
        public virtual SystemEntity Sender { get; set; }
        public virtual ChatRoom ChatRoom { get; set; }
    }
}
