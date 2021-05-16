using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface INotificationService
    {
        Task<PagedList<NotificationVM>> GetMyNotifications(Paging paging, Sorting sorting);
    }
}
