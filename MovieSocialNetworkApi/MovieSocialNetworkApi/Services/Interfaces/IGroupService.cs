using MovieSocialNetworkApi.Models;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IGroupService
    {
        Task<GroupVM> GetById(int id);
        Task<PagedList<GroupVM>> GetList(Paging paging, Sorting sorting, string q);
        Task<GroupVM> Create(CreateGroupCommand command);
        Task<AuthenticationInfo> Login(int id);
    }
}
