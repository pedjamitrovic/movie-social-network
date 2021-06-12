using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Helpers
{
    public class AppSettings
    {
        public string JwtSecret { get; set; }
        public string PwSecret { get; set; }
        public string TmdbApiKey { get; set; }
        public string TmdbBaseUrl { get; set; }
        public int MinReportsCount { get; set; }
    }
}
