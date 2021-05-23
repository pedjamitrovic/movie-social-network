using MovieSocialNetworkApi.Models;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Hubs
{
    public interface IChatHub
    {
        // Client calls
        Task SendMessage(CreateMessageCommand command);
        Task SetMessageSeen(int messageId);
        Task SetNotificationSeen(int notificationId);
        // Server notifies
        Task NotifyChatRoomCreated(ChatRoomVM chatRoomVM);
        Task NotifyMessageCreated(MessageVM messageVM);
        Task NotifyMessageSeen(MessageVM messageVM);
        Task NotifyNewNotification(NotificationVM notificationVM);
    }
}
