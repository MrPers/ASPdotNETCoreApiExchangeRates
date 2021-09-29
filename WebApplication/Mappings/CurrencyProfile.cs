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
            CreateMap<CurrencyHistoryVM, CurrencyHistory>()
                //.ForMember(dst => dst.CurrencyId, opt => opt.Ignore())
                //.ForPath(dst => dst.Currency, opt => opt.Ignore())
                .ReverseMap();

            //CreateMap<Currency, CurrencyModelDto>()
            //    .ReverseMap();
        }
    }
}