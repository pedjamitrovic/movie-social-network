using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class CreateCommentCommand
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
