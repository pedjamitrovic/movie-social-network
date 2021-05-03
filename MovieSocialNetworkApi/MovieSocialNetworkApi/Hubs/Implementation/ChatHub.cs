using Microsoft.AspNetCore.SignalR;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System;

namespace MovieSocialNetworkApi.Hubs
{
    public class ChatHub: Hub<IChatHub>
    {
        public void SendMessage(CreateMessageCommand command)
        {
            var chatRoom = new ChatRoom
            {
                Id = 1,
                Memberships = new ChatRoomMembership[] {
                    new ChatRoomMembership { ChatRoomId = 1, MemberId = 3 },
                    new ChatRoomMembership { ChatRoomId = 1, MemberId = 4 },
                }
            };
            var messageVM = new MessageVM
            {
                Id = command.ChatRoomId,
                ChatRoomId = chatRoom.Id,
                CreatedOn = DateTimeOffset.UtcNow,
                Delivered = false,
                Seen = false,
                Text = command.Text
            };
            Clients.Users("3", "4").ReceiveMessage(messageVM);
        }
    }
}
