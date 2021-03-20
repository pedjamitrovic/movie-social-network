﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class CreatePostCommand
    {
        [Required]
        public string Text { get; set; }
        public IFormFile File { get; set; }
    }
}
