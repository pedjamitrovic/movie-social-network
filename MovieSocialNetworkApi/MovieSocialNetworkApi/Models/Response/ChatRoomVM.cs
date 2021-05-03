using MovieSocialNetworkApi.Entities;
using System.Collections.Generic;

namespace MovieSocialNetworkApi.Models
{
    public class ChatRoomVM
    {
        public int Id { get; set; }
        public ICollection<ChatRoomMembership> Memberships { get; set; }
        public MessageVM NewestMessage { get; set; }
    }
}
