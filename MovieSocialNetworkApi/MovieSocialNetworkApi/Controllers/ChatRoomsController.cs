using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Hubs;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ChatRoomsController : Controller
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;
        private readonly IAuthService _authService;

        public ChatRoomsController(
            IChatRoomService chatRoomService,
            IHubContext<ChatHub, IChatHub> hubContext,
            IAuthService authService
        )
        {
            _chatRoomService = chatRoomService;
            _hubContext = hubContext;
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
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateChatRoomCommand command)
        {
            try
            {
                var authUser = await _authService.GetAuthenticatedSystemEntity();
                var result = await _chatRoomService.Create(command.MemberIds);

                foreach (var memberId in command.MemberIds)
                {
                    if (memberId == authUser.Id) continue;
                    await _hubContext.Clients.User(memberId.ToString()).NotifyChatRoomCreated(result);
                }

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
