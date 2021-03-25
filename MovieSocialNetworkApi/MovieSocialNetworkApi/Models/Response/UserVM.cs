namespace MovieSocialNetworkApi.Models
{
    public class UserVM: SystemEntityVM
    {
        public string Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}