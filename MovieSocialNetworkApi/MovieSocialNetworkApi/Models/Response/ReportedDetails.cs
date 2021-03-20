using System.Collections.Generic;

namespace MovieSocialNetworkApi.Models.Response
{
    public class ReportedDetails
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public List<ReportedStats> ReportedStats { get; set; }
    }
}
