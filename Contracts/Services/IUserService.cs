using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Services
{
    public interface IUserService
    {
        Task<long> RegisterAsync(UserDto userModel);
    }
}