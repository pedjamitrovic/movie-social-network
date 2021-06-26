using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RecommendationsController : Controller
    {
        private readonly IMovieService _movieService;

        public RecommendationsController(
            IMovieService movieService
        )
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendations([FromQuery] Paging paging)
        {
            var movies = await _movieService.GetRecommendations(paging);
            return Ok(movies);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("calculate")]
        public IActionResult CalculateRecommendations()
        {
            BackgroundJob.Enqueue(() => _movieService.CalculateRecommendations());
            return NoContent();
        }
    }
}
