using AutoMapper;
using MovieSocialNetworkApi.Models;
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
        public override ReportedDetails GetReportedDetails(IMapper mapper)
        {
            var reportedDetails = new ReportedDetails
            {
                Discriminator = nameof(Group),
                Extended = mapper.Map<GroupVM>(this),
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
