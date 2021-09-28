using AutoMapper;
using WebApplication.DTO;
using WebApplication.Entities;
using WebApplication.Models;
//using WebApplication.Models;

namespace WebApplication.Mappings
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CurrencyModelDto, CurrencyModelDto>().ReverseMap();
            CreateMap<Currency, CurrencyModelDto>().ReverseMap();
        }
    }
}