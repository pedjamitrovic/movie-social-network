using MovieSocialNetworkApi.Models.Response;
using System.Linq;

namespace MovieSocialNetworkApi.Entities
{
    public class Comment : Content
    {
        public virtual Post Post { get; set; }
        public override ReportedDetails GetReportedDetails()
        {
            var reportedDetails = new ReportedDetails
            {
                Type = nameof(Comment),
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
