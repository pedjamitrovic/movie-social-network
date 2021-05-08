using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class CreateChatRoomCommand
    {
        [Required]
        public ICollection<int> MemberIds { get; set; }
    }
}
