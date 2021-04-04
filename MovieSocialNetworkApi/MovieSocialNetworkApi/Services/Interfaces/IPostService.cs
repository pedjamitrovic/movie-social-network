using MovieSocialNetworkApi.Models;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IPostService
    {
        Task<PagedList<PostVM>> GetList(Paging paging, Sorting sorting, string q, int? creatorId, int? followerId);
        Task<PostVM> GetById(int id);
        Task<PostVM> Create(string filePath, CreatePostCommand command);
    }
}
