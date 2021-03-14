using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Models
{
    public class UserVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ProfileImagePath { get; set; }
        public string CoverImagePath { get; set; }
        public string Username { get; set; }
    }
}