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
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IFileService _fileService;

        public PostsController(
            IPostService postService,
            IFileService fileService
        )
        {
            _postService = postService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery] Paging paging, 
            [FromQuery] Sorting sorting, 
            [FromQuery] string q, 
            [FromQuery] int? creatorId, 
            [FromQuery] int? followerId
        )
        {
            try
            {
                var result = await _postService.GetList(paging, sorting, q, creatorId, followerId);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _postService.GetById(id);
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

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] CreatePostCommand command)
        {
            try
            {
                var filePath = await _fileService.SaveFile(command.File);
                var result = await _postService.Create(filePath, command);
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
