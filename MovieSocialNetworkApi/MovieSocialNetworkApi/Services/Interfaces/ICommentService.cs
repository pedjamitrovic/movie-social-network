using MovieSocialNetworkApi.Models;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface ICommentService
    {
        Task<PagedList<CommentVM>> GetList(Paging paging, Sorting sorting, string q);
        Task<CommentVM> GetById(int id);
        Task<CommentVM> Create(string filePath, CreateCommentCommand command);
    }
}
