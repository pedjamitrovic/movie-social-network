using Microsoft.EntityFrameworkCore;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Entities
{
    public class User: SystemEntity
    {
        public string Role { get; set; }
        [MinLength(3)]
        public string Username { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public virtual ICollection<GroupAdmin> GroupAdmin { get; set; }
        public override ReportedDetails GetReportedDetails()
        {
            var reportedDetails = new ReportedDetails
            {
                Type = nameof(User),
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
