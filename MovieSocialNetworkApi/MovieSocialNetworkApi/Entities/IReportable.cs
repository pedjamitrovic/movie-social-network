using AutoMapper;
using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;

namespace MovieSocialNetworkApi.Entities
{
    public interface IReportable
    {
        public abstract ICollection<Report> ReportedReports { get; set; }
        ReportedDetails GetReportedDetails(IMapper mapper);
    }
}
