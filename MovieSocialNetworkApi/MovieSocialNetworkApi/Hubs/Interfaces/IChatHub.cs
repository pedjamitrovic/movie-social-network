using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Hubs
{
    public interface IChatHub
    {
        Task SendMessage(int receiverId, string message);
        Task ReceiveMessage(string message);
    }
}
