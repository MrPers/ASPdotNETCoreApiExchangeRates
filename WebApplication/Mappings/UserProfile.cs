using AutoMapper;
using WebApplication.DTO;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //CreateMap<UserViewModel, UserModelDto>().ReverseMap();
            //CreateMap<User, UserModelDto>().ReverseMap();

            CreateMap<UserViewVM, UserModelDto>().ReverseMap();
            CreateMap<User, UserModelDto>().ReverseMap()
            //.ForMember(dest => dest.Authorize, opt => opt.Ignore())
            ;
        }
    }
}