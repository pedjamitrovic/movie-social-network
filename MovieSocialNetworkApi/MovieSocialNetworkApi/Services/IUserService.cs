using MovieSocialNetworkApi.Models;
using System.Collections.Generic;

namespace MovieSocialNetworkApi.Services
{
    public interface IUserService
    {
        AuthenticatedUser Authenticate(AuthenticateCommand command);
        IEnumerable<UserVM> GetAll();
        UserVM GetById(int id);
    }
}
