using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IContentService
    {
        Task<ReactionVM> React(int id, CreateReactionCommand command);
        Task Report(int id, ReportCommand command);
        Task Delete(int id);
        Task<PagedList<ReportedDetails>> GetBannable(Paging paging, Sorting sorting);
    }
}
