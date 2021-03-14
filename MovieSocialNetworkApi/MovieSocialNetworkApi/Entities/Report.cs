using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public string Reason { get; set; }
        public int ReporterId { get; set; }
        public int ReportedSystemEntityId { get; set; }
        public virtual SystemEntity Reporter { get; set; }
        public virtual SystemEntity ReportedSystemEntity { get; set; }
        public virtual Content ReportedContent { get; set; }

        public IReportable Reported { 
            get
            {
                if (ReportedSystemEntity != null)
                {
                    return ReportedSystemEntity;
                } 
                if (ReportedContent != null)
                {
                    return ReportedContent;
                }
                throw new ApplicationException("Unknown reported type");
            } 
        }
    }
}
