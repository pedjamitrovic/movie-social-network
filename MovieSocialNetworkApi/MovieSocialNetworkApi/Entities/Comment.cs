using MovieSocialNetworkApi.Models.Response;

namespace MovieSocialNetworkApi.Entities
{
    public class Comment : Content
    {
        public virtual Post Post { get; set; }
        public override ReportedDetails GetDetails()
        {
            var reportedDetails = new ReportedDetails();
            reportedDetails.Details.Add("type", nameof(Comment));
            reportedDetails.Details.Add("reportedId", Id.ToString());
            return reportedDetails;
        }
    }
}
