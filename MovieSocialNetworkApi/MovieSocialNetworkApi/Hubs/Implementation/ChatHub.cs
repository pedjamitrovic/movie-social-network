using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MovieSocialNetworkApi.Hubs
{
    public class ChatHub: Hub<IChatHub>
    {
        public void SendMessage(int receiverId, string message)
        {
            var user = Clients.User(receiverId.ToString());
            user.ReceiveMessage($"{Context.User.Identity.Name} sent you a message: {message}");
        }
    }
}
