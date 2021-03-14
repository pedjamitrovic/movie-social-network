using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Models.Response
{
    public class ReportedDetails
    {
        public Dictionary<string, string> Details { get; } = new Dictionary<string, string>();
    }
}
