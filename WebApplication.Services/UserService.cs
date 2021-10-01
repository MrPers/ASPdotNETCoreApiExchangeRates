using AutoMapper;
//using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entites;
using WebApplication.Repository;

namespace WebApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //public string Authenticate(string login, string password)
        //{
        //    var user = _userRepository
        //        .GetAll()
        //        .FirstOrDefault(x => x.Name == login && x.Password == password);

        //    if (user == null)
        //    {
        //        // todo: need to add logger
        //        return null;
        //    }

        //    var token = _configuration.GenerateJwtToken(user);

        //    return token ;
        //}

        public async Task<UserModelDto> Register(UserModelDto userModel)
        {
            var user = _mapper.Map<User>(userModel);

            var newUserId = await _userRepository.Add(user);

            var newUser = new UserModelDto
            {
                Id = newUserId,
                Name = user.Name,
                Password = user.Password
            };

            return newUser;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }
    }
}