using MovieSocialNetworkApi.Models.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Entities
{
    public abstract class SystemEntity: IReportable
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string ProfileImagePath { get; set; }
        public string CoverImagePath { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Relation> Following { get; set; }
        public virtual ICollection<Relation> Followers { get; set; }
        public virtual ICollection<Report> ReporterReports { get; set; }
        public virtual ICollection<Report> ReportedReports { get; set; }
        public virtual ICollection<Ban> Bans { get; set; }
        public abstract ReportedDetails GetDetails();
    }
}