using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entites;

namespace WebApplication.Services
{
    public interface IUserService
    {
        //AuthenticateResponse Authenticate(string login, string password);
        Task<UserModelDto> Register(UserModelDto userModel);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}