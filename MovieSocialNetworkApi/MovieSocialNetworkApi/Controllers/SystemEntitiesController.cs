using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SystemEntitiesController : Controller
    {
        private readonly ISystemEntityService _systemEntityService;
        private readonly IFileService _fileService;

        public SystemEntitiesController(
            ISystemEntityService systemEntityService,
            IFileService fileService
        )
        {
            _fileService = fileService;
            _systemEntityService = systemEntityService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetList([FromQuery] Paging paging, [FromQuery] Sorting sorting, [FromQuery] string q)
        //{
        //    try
        //    {
        //        var result = await _systemEntityService.GetList(paging, sorting, q);
        //        return Ok(result);
        //    }
        //    catch (BusinessException e)
        //    {
        //        return BadRequest(new { message = e.Message });
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var user = await _systemEntityService.GetById(id);

        //    if (user == null) return NotFound();

        //    return Ok(user);
        //}

        [HttpPut("{id}/follow")]
        public async Task<IActionResult> Follow(int id)
        {
            try
            {
                await _systemEntityService.Follow(id);
                return NoContent();
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("{id}/unfollow")]
        public async Task<IActionResult> Unfollow(int id)
        {
            try
            {
                await _systemEntityService.Unfollow(id);
                return NoContent();
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}/followers")]
        public async Task<IActionResult> GetFollowers(int id, [FromQuery] Paging paging, [FromQuery] Sorting sorting)
        {
            try
            {
                var followers = await _systemEntityService.GetFollowers(id, paging, sorting);
                return Ok(followers);
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}/following")]
        public async Task<IActionResult> GetFollowing(int id, [FromQuery] Paging paging, [FromQuery] Sorting sorting)
        {
            try
            {
                var following = await _systemEntityService.GetFollowing(id, paging, sorting);
                return Ok(following);
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("{id}/image/{type}"), DisableRequestSizeLimit]
        public async Task<IActionResult> ChangeImage(int id, string type, [FromForm] IFormFile file)
        {
            try
            {
                var filePath = await _fileService.SaveFile(file);
                await _systemEntityService.ChangeImage(id, type, filePath);
                return Ok(filePath);
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

        [HttpPatch("{id}/description")]
        public async Task<IActionResult> ChangeDescription(int id, ChangeDescriptionCommand command)
        {
            try
            {
                await _systemEntityService.ChangeDescription(id, command);
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
