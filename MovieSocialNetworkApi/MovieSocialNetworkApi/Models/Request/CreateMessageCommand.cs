using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class CreateMessageCommand
    {
        public int ChatRoomId { get; set; }
        public string Text { get; set; }
    }
}
