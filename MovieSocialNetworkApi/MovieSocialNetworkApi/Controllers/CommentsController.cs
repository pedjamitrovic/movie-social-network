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
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(
            ICommentService commentService
        )
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] Paging paging, [FromQuery] Sorting sorting, [FromQuery] string q, [FromQuery] int? creatorId, [FromQuery] int? postId)
        {
            try
            {
                var result = await _commentService.GetList(paging, sorting, q, creatorId, postId);
                return Ok(result);
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _commentService.GetById(id);
                return Ok(result);
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Create(CreateCommentCommand command)
        {
            try
            {
                var result = await _commentService.Create(command);
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
    }
}
