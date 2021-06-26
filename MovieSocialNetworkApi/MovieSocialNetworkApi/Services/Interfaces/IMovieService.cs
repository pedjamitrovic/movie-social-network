using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IMovieService
    {
        Task<object> GetConfiguration();
        Task<object> GetMovieDetails(int id);
        Task<object> SearchMovies(string query);
        Task<object> GetTrendingMovies(string timeWindow);
        Task<object> GetPopularMovies(Paging paging);
        Task<object> GetMovieKeywords(int id);
        Task<object> GetMovieRecommendations(int id);
        Task<object> GetMovieCredits(int id);
        Task<object> GetSimilarMovies(int id);
        Task<object> GetMovieVideos(int id);
        Task<MovieRatingVM> GetMyMovieRating(int movieId);
        Task<MovieRatingVM> RateMovie(int movieId, RateMovieCommand command);
        Task CalculateRecommendations();
        Task CalculateTempRecommendations();
        Task ActivateTempRecommendations();
        Task<object> GetRecommendations(Paging paging);
    }
}