using AutoMapper;
using MovieSocialNetworkApi.Models.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public abstract class Content: IReportable
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public virtual SystemEntity Creator { get; set; }
        public virtual ICollection<Reaction> Reactions { get; set; }
        public virtual ICollection<Report> ReportedReports { get; set; }
        public abstract ReportedDetails GetReportedDetails(IMapper mapper);
    }
}
