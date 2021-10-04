using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Repository
{
    public interface IUserRepository : IBaseRepository<UserDto, UserDto, long>
    {
        Task<long> Add(UserDto userDto);
    }
}
