using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class ChatRoomMembership
    {
        public int ChatRoomId { get; set; }
        public int MemberId { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual SystemEntity Member { get; set; }
    }
}
