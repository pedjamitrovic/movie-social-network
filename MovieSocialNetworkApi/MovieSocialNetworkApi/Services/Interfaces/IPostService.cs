using MovieSocialNetworkApi.Models;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IPostService
    {
        Task<PagedList<PostVM>> GetList(Paging paging, Sorting sorting, string q);
        Task<PostVM> GetById(int id);
        Task<PostVM> Create(string filePath, CreatePostCommand command);
    }
}
