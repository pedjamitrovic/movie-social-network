using System;

namespace MovieSocialNetworkApi.Models
{
    public class MessageVM
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ChatRoomId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool Delivered { get; set; }
        public bool Seen { get; set; }
    }
}
