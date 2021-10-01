using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entites;
//using WebApplication.Models;

namespace WebApplication.DB.Mappings
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            //CreateMap<UserViewVM, UserModelDto>().ReverseMap();
            //CreateMap<User, UserModelDto>().ReverseMap();
        }
    }
}