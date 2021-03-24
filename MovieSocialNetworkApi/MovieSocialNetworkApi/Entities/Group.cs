using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MovieSocialNetworkApi.Entities
{
    public class Group : SystemEntity
    {
        [MinLength(3)]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public virtual ICollection<GroupAdmin> GroupAdmin { get; set; }
        public override ReportedDetails GetReportedDetails()
        {
            var reportedDetails = new ReportedDetails
            {
                Type = nameof(Group),
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
