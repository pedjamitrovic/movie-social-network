using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IUserService
    {
        Task<UserVM> GetById(int id);
        Task<PagedList<UserVM>> GetList(Paging paging, Sorting sorting, string q);
        Task<AuthenticationInfo> Register(RegisterCommand command);
        Task<AuthenticationInfo> Login(LoginCommand command);
    }
}
