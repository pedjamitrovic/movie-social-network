using Microsoft.AspNetCore.SignalR;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using System;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Hubs
{
    public class ChatHub: Hub<IChatHub>
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatHub(
            IChatRoomService chatRoomService
        )
        {
            _chatRoomService = chatRoomService;
        }

        public async Task SendMessage(CreateMessageCommand command)
        {
            var messageVM = await _chatRoomService.CreateMessage(command);
            var members = await _chatRoomService.GetMembers(command.ChatRoomId);
            foreach (var member in members)
            {
                await Clients.User(member.Id.ToString()).ReceiveMessage(messageVM);
            }
        }
    }
}
