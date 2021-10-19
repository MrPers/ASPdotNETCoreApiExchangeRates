using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DB.Entites;
using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.Mappings
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserViewVM, UserDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CurrencyHistoryVM, CurrencyHistoryDto>().ReverseMap();
            CreateMap<CurrencyHistory, CurrencyHistoryDto>().ReverseMap();
            CreateMap<AnswerCurrencyHistory, CurrencyHistoryDto>();
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<CurrencyVM, CurrencyDto>().ReverseMap();
        }
    }
}