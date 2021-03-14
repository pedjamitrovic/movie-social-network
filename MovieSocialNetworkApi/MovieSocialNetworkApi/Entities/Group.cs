using MovieSocialNetworkApi.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Entities
{
    public class Group : SystemEntity
    {
        [MinLength(3)]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public override ReportedDetails GetDetails()
        {
            var reportedDetails = new ReportedDetails();
            reportedDetails.Details.Add("type", nameof(Group));
            reportedDetails.Details.Add("reportedId", Id.ToString());
            return reportedDetails;
        }
    }
}
