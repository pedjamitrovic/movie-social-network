using MovieSocialNetworkApi.Entities;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IAuthService
    {
        Task<SystemEntity> GetAuthenticatedSystemEntity();
    }
}
