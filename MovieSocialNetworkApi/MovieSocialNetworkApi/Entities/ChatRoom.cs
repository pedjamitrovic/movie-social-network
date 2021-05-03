using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<ChatRoomMembership> Memberships { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
