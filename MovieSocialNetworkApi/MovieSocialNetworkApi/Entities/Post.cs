using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;
using System.Linq;

namespace MovieSocialNetworkApi.Entities
{
    public class Post: Content
    {
        public string FilePath { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public override ReportedDetails GetReportedDetails()
        {
            var reportedDetails = new ReportedDetails
            {
                Type = nameof(Post),
                Id = Id,

                ReportedStats = ReportedReports
                .GroupBy(e => e.Reason)
                .Select(g => new ReportedStats { Reason = g.Key, Count = g.Count() })
                .OrderByDescending(rs => rs.Count)
                .ToList()
            };

            return reportedDetails;
        }
    }
}
