using Microsoft.AspNetCore.Mvc;

namespace MovieSocialNetworkApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok("Application is running...");
        }
    }
}
