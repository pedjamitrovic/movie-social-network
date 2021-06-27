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
        private readonly IRecommendationService _recommendationService;

        public RecommendationsController(
            IMovieService movieService,
            IRecommendationService recommendationService
        )
        {
            _movieService = movieService;
            _recommendationService = recommendationService;
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

        [Authorize(Roles = Role.Admin)]
        [HttpPost("matrix-factorization")]
        public IActionResult CreateMatrixFactorizationModel()
        {
            _recommendationService.CreateModel();
            return NoContent();
        }
    }
}
