using System.Collections.Generic;

namespace MovieSocialNetworkApi.Models.Response
{
    public class ReportedDetails
    {
        public List<ReportedStats> ReportedStats { get; set; }
        public string Discriminator { get; set; }
        public object Extended { get; set; }
    }
}
