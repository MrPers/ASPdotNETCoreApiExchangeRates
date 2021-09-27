using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IUserService
    {
        Task<string> Authenticate(string login, string password);
        Task<UserModelDto> Register(UserModelDto userModel);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}