﻿using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class ReportCommand
    {
        [Required]
        public string Reason { get; set; }
    }
}
