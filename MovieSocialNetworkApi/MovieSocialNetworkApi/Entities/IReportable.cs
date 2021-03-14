using MovieSocialNetworkApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Entities
{
    public interface IReportable
    {
        public abstract ICollection<Report> ReportedReports { get; set; }
        ReportedDetails GetDetails();
    }
}
