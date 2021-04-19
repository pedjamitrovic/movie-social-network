namespace MovieSocialNetworkApi.Models
{
    public class GroupVM : SystemEntityVM
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public bool IsAuthUserAdmin { get; set; }
    }
}
