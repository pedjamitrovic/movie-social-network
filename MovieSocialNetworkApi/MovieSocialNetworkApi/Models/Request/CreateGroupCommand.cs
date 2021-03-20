using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class CreateGroupCommand
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
    }
}
