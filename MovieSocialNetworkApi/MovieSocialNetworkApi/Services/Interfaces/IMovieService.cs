using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IMovieService
    {
        Task<object> GetConfiguration();
        Task<object> GetMovieDetails(int id);
        Task<object> SearchMovies(string query);
        Task<object> GetTrendingMovies(string timeWindow);
        Task<object> GetPopularMovies();
    }
}