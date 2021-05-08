using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieSocialNetworkApi.Exceptions;
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

        public ChatRoomsController(
            IChatRoomService chatRoomService
        )
        {
            _chatRoomService = chatRoomService;
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
        public async Task<IActionResult> Create(ICollection<int> memberIds)
        {
            try
            {
                var result = await _chatRoomService.Create(memberIds);
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
