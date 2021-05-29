using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContentsController : Controller
    {
        private readonly IContentService _contentService;

        public ContentsController(
            IContentService contentService
        )
        {
            _contentService = contentService;
        }

        [HttpPost("{id}/reaction")]
        public async Task<IActionResult> React(int id, CreateReactionCommand command)
        {
            try
            {
                var result = await _contentService.React(id, command);
                return Ok(result);
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

        [HttpPost("{id}/report"), DisableRequestSizeLimit]
        public async Task<IActionResult> Report(int id, ReportCommand command)
        {
            try
            {
                await _contentService.Report(id, command);
                return Ok();
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message, code = e.Code });
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
        }
    }
}
