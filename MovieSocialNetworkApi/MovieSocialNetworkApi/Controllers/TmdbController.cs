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
    }
}
