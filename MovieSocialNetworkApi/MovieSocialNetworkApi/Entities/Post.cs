using AutoMapper;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;
using System.Linq;

namespace MovieSocialNetworkApi.Entities
{
    public class Post: Content
    {
        public string FilePath { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Group ForGroup { get; set; }
        public override ReportedDetails GetReportedDetails(IMapper mapper)
        {
            var reportedDetails = new ReportedDetails
            {
                Discriminator = nameof(Post),
                Extended = mapper.Map<PostVM>(this),
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
