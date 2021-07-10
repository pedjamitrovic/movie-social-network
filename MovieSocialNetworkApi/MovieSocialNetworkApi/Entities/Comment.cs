using AutoMapper;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System.Linq;

namespace MovieSocialNetworkApi.Entities
{
    public class Comment : Content
    {
        public virtual Post Post { get; set; }
        public override ReportedDetails GetReportedDetails(IMapper mapper)
        {
            var reportedDetails = new ReportedDetails
            {
                Discriminator = nameof(Comment),
                Extended = mapper.Map<CommentVM>(this),
                ReportedStats = ReportedReports
                .Where(e => !e.Reviewed)
                .GroupBy(e => e.Reason)
                .Select(g => new ReportedStats { Reason = g.Key, Count = g.Count() })
                .OrderByDescending(rs => rs.Count)
                .ToList()
            };

            return reportedDetails;
        }
    }
}
