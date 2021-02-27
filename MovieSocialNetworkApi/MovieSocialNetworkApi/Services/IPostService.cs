using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IPostService
    {
        public Task<PagedList<Post>> GetList(Paging paging, Sorting sorting, string q);
    }
}
