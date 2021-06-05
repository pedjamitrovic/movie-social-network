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
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(
            IUserService userService
        )
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] Paging paging, [FromQuery] Sorting sorting, [FromQuery] string q)
        {
            try
            {
                var result = await _userService.GetList(paging, sorting, q);
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
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetById(id);

                if (user == null) return NotFound();

                return Ok(user);
            }
            catch (BusinessException e)
            {
                return BadRequest(new { message = e.Message, code = e.Code });
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch
            {
                return BadRequest();
            }
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
                return BadRequest(new { message = e.Message, code = e.Code });
            }
            catch (ForbiddenException)
            {
                return Forbid();
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
                return BadRequest(new { message = e.Message, code = e.Code });
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
