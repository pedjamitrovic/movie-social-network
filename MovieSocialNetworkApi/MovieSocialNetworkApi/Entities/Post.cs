using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;

namespace MovieSocialNetworkApi.Entities
{
    public class Post: Content
    {
        public string FilePath { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Group OwnerGroup { get; set; }
        // Media
        public override ReportedDetails GetDetails()
        {
            var reportedDetails = new ReportedDetails();
            reportedDetails.Details.Add("type", nameof(Post));
            reportedDetails.Details.Add("reportedId", Id.ToString());
            return reportedDetails;
        }
    }
}
