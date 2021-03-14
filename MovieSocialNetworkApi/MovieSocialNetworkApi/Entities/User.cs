using MovieSocialNetworkApi.Models.Response;
using System.ComponentModel.DataAnnotations;

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
        public override ReportedDetails GetDetails()
        {
            var reportedDetails = new ReportedDetails();
            reportedDetails.Details.Add("type", nameof(User));
            reportedDetails.Details.Add("reportedId", Id.ToString());
            return reportedDetails;
        }
    }
}
