﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MovieSocialNetworkApi.Models
{
    public class BanCommand
    {
        [Required]
        public DateTimeOffset BannedUntil { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}
