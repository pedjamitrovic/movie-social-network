namespace MovieSocialNetworkApi.Models
{
    public class SystemEntityVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ProfileImagePath { get; set; }
        public string CoverImagePath { get; set; }
        public string QualifiedName { get; set; }
        public string Discriminator { get; set; }
    }
}
