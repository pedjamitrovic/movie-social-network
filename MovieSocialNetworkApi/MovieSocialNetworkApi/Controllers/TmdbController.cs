using Microsoft.AspNetCore.Mvc;
using MovieSocialNetworkApi.Services;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TmdbController : Controller
    {
        private readonly IMovieService _movieService;

        public TmdbController(
            IMovieService movieService
        )
        {
            _movieService = movieService;
        }

        [HttpGet("configuration")]
        public async Task<object> GetConfiguration()
        {
            return await _movieService.GetConfiguration();
        }

        [HttpGet("movie/{id}")]
        public async Task<object> GetMovieDetails(int id)
        {
            return await _movieService.GetMovieDetails(id);
        }

        [HttpGet("search/movie")]
        public async Task<object> SearchMovies([FromQuery] string query)
        {
            return await _movieService.SearchMovies(query);
        }

        [HttpGet("trending/movie/{timeWindow}")]
        public async Task<object> GetTrendingMovies(string timeWindow)
        {
            return await _movieService.GetTrendingMovies(timeWindow);
        }

        [HttpGet("movie/popular")]
        public async Task<object> GetPopularMovies()
        {
            return await _movieService.GetPopularMovies();
        }

        [HttpGet("movie/{id}/keywords")]
        public async Task<object> GetMovieKeywords(int id)
        {
            return await _movieService.GetMovieKeywords(id);
        }

        [HttpGet("movie/{id}/recommendations")]
        public async Task<object> GetMovieRecommendations(int id)
        {
            return await _movieService.GetMovieRecommendations(id);
        }

        [HttpGet("movie/{id}/credits")]
        public async Task<object> GetMovieCredits(int id)
        {
            return await _movieService.GetMovieCredits(id);
        }

        [HttpGet("movie/{id}/similar")]
        public async Task<object> GetSimilarMovies(int id)
        {
            return await _movieService.GetSimilarMovies(id);
        }

        [HttpGet("movie/{id}/videos")]
        public async Task<object> GetMovieVideos(int id)
        {
            return await _movieService.GetMovieVideos(id);
        }
    }
}
