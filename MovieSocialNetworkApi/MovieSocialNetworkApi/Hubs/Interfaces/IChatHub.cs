using MovieSocialNetworkApi.Models;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Hubs
{
    public interface IChatHub
    {
        Task SendMessage(CreateMessageCommand command);
        Task ReceiveMessage(MessageVM messageVM);
        Task ChatRoomCreated(ChatRoomVM chatRoomVM);
    }
}
