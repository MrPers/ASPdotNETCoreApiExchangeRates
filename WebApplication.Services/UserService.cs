using AutoMapper;
//using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DB.Entites;
using WebApplication.DTO;
using WebApplication.Repository;

namespace WebApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<long> RegisterAsync(UserDto userModel)
        {
            return await _userRepository.Add(userModel);
        }
    }
}