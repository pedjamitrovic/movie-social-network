using Microsoft.AspNetCore.SignalR;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Services;
using System;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Hubs
{
    public class MovieSocialNetworkHub: Hub<IMovieSocialNetworkHub>
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly INotificationService _notificationService;

        public MovieSocialNetworkHub(
            IChatRoomService chatRoomService,
            INotificationService notificationService
        )
        {
            _chatRoomService = chatRoomService;
            _notificationService = notificationService;
        }

        public async Task SendMessage(CreateMessageCommand command)
        {
            var messageVM = await _chatRoomService.CreateMessage(command);
            var members = await _chatRoomService.GetMembers(command.ChatRoomId);
            foreach (var member in members)
            {
                await Clients.User(member.Id.ToString()).NotifyMessageCreated(messageVM);
            }
        }
        public async Task SetMessageSeen(int messageId)
        {
            var messageVM = await _chatRoomService.SetMessageSeen(messageId);
            var members = await _chatRoomService.GetMembers(messageVM.ChatRoomId);
            foreach (var member in members)
            {
                await Clients.User(member.Id.ToString()).NotifyMessageSeen(messageVM);
            }
        }
        public async Task SetNotificationSeen(int notificationId)
        {
            await _notificationService.SetNotificationSeen(notificationId);
        }
    }
}
