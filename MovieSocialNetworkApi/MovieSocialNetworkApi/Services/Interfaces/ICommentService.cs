using MovieSocialNetworkApi.Models;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface ICommentService
    {
        Task<PagedList<CommentVM>> GetList(Paging paging, Sorting sorting, string q, int? creatorId, int? postId);
        Task<CommentVM> GetById(int id);
        Task<CommentVM> Create(CreateCommentCommand command);
    }
}
