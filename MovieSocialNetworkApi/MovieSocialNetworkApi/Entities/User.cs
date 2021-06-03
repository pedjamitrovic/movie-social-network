using AutoMapper;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MovieSocialNetworkApi.Entities
{
    public class User: SystemEntity
    {
        [MinLength(3)]
        public string Username { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public virtual ICollection<GroupAdmin> GroupAdmin { get; set; }
        public override ReportedDetails GetReportedDetails(IMapper mapper)
        {
            var reportedDetails = new ReportedDetails
            {
                Discriminator = nameof(User),
                Extended = mapper.Map<UserVM>(this),
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
