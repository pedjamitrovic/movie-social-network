using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ChatRoomsController : Controller
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IAuthService _authService;

        public ChatRoomsController(
            IChatRoomService chatRoomService,
            IAuthService authService
        )
        {
            _chatRoomService = chatRoomService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyChatRooms([FromQuery] Paging paging, [FromQuery] Sorting sorting)
        {
            try
            {
                var result = await _chatRoomService.GetMyChatRooms(paging, sorting);
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

        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetMessages([FromQuery] Paging paging, [FromQuery] Sorting sorting, int id)
        {
            try
            {
                var result = await _chatRoomService.GetMessageList(paging, sorting, id);
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateChatRoomCommand command)
        {
            try
            {
                var authUser = await _authService.GetAuthenticatedSystemEntity();
                var result = await _chatRoomService.Create(command.MemberIds);

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
