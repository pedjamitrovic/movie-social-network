namespace MovieSocialNetworkApi.Recommender
{
    public class MovieRating
    {
        public int MovieId { get; set; }
        public int OwnerId { get; set; }
        public float Rating { get; set; }
    }
}
