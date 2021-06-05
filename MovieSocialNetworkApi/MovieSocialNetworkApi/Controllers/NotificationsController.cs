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
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(
            INotificationService notificationService
        )
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications([FromQuery] Paging paging, [FromQuery] Sorting sorting)
        {
            try
            {
                var result = await _notificationService.GetMyNotifications(paging, sorting);
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

        [HttpGet("count")]
        public async Task<IActionResult> GetMyUnseenNotificationCount()
        {
            try
            {
                var result = await _notificationService.GetMyUnseenNotificationCount();
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
    }
}
