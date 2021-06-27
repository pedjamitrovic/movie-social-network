using MovieSocialNetworkApi.Recommender;

namespace MovieSocialNetworkApi.Services
{
    public interface IRecommendationService
    {
        MovieRatingPrediction Predict(int userId, int movieId);
        void CreateModel();
    }
}
