using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
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

        [HttpGet("movie/{id}/rating/me")]
        public async Task<object> GetMyMovieRating(int id)
        {
            try
            {
                var result = await _movieService.GetMyMovieRating(id);

                if (result != null)
                {
                    return Ok(result);
                } else
                {
                    return NotFound();
                }
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
        }

        [HttpPost("movie/{id}/rating")]
        public async Task<IActionResult> RateMovie(int id, [FromBody] RateMovieCommand command)
        {
            try
            {
                var result = await _movieService.RateMovie(id, command);
                return NoContent();
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
        }
    }
}
