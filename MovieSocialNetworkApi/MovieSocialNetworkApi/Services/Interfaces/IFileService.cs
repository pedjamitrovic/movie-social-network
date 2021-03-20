using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file);
    }
}
