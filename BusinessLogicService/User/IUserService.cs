using Contracts;
using Contracts.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.User
{
    public interface IUserService
    {
        Task<Result<string>> LoginAsync(string user, string pass);
        Result<string> Login(string user, string pass);
    }
}
