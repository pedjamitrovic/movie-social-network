using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public abstract class SystemEntity: IReportable
    {
        [Key]
        public int Id { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public string ProfileImagePath { get; set; }
        public string CoverImagePath { get; set; }
        public string Discriminator { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual ICollection<Relation> Following { get; set; }
        public virtual ICollection<Relation> Followers { get; set; }
        public virtual ICollection<Report> ReporterReports { get; set; }
        public virtual ICollection<Report> ReportedReports { get; set; }
        public virtual ICollection<Ban> Bans { get; set; }
        public virtual ICollection<ChatRoomMembership> ChatRoomMemberships { get; set; }
        public virtual ICollection<Notification> SentNotifications { get; set; }
        public virtual ICollection<Notification> ReceivedNotifications { get; set; }
        public abstract ReportedDetails GetReportedDetails();
    }
}