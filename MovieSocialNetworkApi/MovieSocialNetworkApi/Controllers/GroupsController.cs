using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupsController(
            IGroupService groupService
        )
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] Paging paging, [FromQuery] Sorting sorting, [FromQuery] string q)
        {
            try
            {
                var result = await _groupService.GetList(paging, sorting, q);
                return Ok(result);
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _groupService.GetById(id);

            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
