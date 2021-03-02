using Microsoft.AspNetCore.Authorization;
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
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var user = await _userService.Login(command);
                return Ok(user);
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var user = await _userService.Register(command);
                return Ok(user);
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

        [HttpPut("{id}/follow")]
        public async Task<IActionResult> Follow(int id)
        {
            try
            {
                await _userService.Follow(id);
                return Ok();
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

        [HttpPut("{id}/unfollow")]
        public async Task<IActionResult> Unfollow(int id)
        {
            try
            {
                await _userService.Unfollow(id);
                return Ok();
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

        [HttpGet("{id}/followers")]
        public async Task<IActionResult> GetFollowers(int id)
        {
            try
            {
                var followers = await _userService.GetFollowers(id);
                return Ok(followers);
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

        [HttpGet("{id}/following")]
        public async Task<IActionResult> GetFollowing(int id)
        {
            try
            {
                var following = await _userService.GetFollowing(id);
                return Ok(following);
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


        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
