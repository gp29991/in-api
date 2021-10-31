using in_api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_api.Services
{
    public interface IUserService
    {
        Task<Response> RegisterUser(RegisterViewModel model);
        Task<Response> LoginUser(LoginViewModel model);
    }
}
