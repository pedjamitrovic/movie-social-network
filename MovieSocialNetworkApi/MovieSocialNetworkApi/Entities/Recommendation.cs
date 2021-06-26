namespace MovieSocialNetworkApi.Entities
{
    public class Recommendation
    {
        public int MovieId { get; set; }
        public int OwnerId { get; set; }
        public int Rating { get; set; }
        public virtual SystemEntity Owner { get; set; }
    }
}
