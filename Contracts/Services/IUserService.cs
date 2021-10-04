using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Services
{
    public interface IUserService
    {
        //AuthenticateResponse Authenticate(string login, string password);
        Task<UserDto> Register(UserDto userModel);
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
    }
}