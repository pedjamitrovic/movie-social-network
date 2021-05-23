using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface INotificationService
    {
        Task<PagedList<NotificationVM>> GetMyNotifications(Paging paging, Sorting sorting);
        Task<NotificationVM> CreateNotification(string type, int senderId, int recepientId, Dictionary<string, object> extended = null);
        Task<int> GetMyUnseenNotificationCount();
        Task<NotificationVM> SetNotificationSeen(int notificationId);
    }
}
