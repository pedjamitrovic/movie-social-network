using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class RateMovieCommand
    {
        public int? Rating { get; set; }
    }
}
