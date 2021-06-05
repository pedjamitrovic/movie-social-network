using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface ISystemEntityService
    {
        Task<SystemEntityVM> GetById(int id);
        Task<PagedList<SystemEntityVM>> GetList(Paging paging, Sorting sorting, string q);
        Task Report(int id, ReportCommand command);
        Task ChangeImage(int id, string type, string imagePath);
        Task ChangeDescription(int id, ChangeDescriptionCommand command);
        Task Follow(int id);
        Task Unfollow(int id);
        Task<PagedList<SystemEntityVM>> GetFollowers(int id, Paging paging, Sorting sorting);
        Task<PagedList<SystemEntityVM>> GetFollowing(int id, Paging paging, Sorting sorting);
        Task<PagedList<PostVM>> GetPosts(int id, Paging paging, Sorting sorting);
        Task<PagedList<CommentVM>> GetComments(int id, Paging paging, Sorting sorting);
        Task<PagedList<ReportedDetails>> GetBannable(Paging paging, Sorting sorting);
        Task ReviewReport(int id, ReviewReportCommand command);
    }
}
