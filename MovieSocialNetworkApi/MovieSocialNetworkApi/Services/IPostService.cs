using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IPostService
    {
        public Task<PagedList<Post>> GetList(Paging paging, Sorting sorting, string q);
        public Task<Post> GetById(int id);
        public Task<Post> Create(CreatePostCommand command);
        public Task<bool> React(CreateReactionCommand command);
        public Task<bool> Report(ReportCommand command);
        public Task<bool> Delete(int id);
    }
}
