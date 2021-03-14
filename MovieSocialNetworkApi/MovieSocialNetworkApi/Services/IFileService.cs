using Microsoft.AspNetCore.Http;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IFileService
    {
        public Task<string> SaveFile(IFormFile file);
    }
}
