using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IUserService
    {
        public Task<UserVM> GetById(int id);
        public Task<PagedList<UserVM>> GetList(Paging paging, Sorting sorting, string q);
        public Task<AuthenticatedUser> Register(RegisterCommand command);
        public Task<AuthenticatedUser> Login(LoginCommand command);
    }
}
