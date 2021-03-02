using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IAuthService
    {
        public Task<AbstractUser> GetAuthenticatedUser();
    }
}
