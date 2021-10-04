using AutoMapper;
using System.Threading.Tasks;
using WebApplication.DB;
using WebApplication.DB.Entites;
using WebApplication.DTO;

namespace WebApplication.Repository
{
    public class UserRepository : BaseRepository<User, UserDto, long>, IUserRepository
    {
        public UserRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<long> Add(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

    }
}