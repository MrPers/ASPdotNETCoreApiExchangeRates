using AutoMapper;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Mappings
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CurrencyHistoryModel, CurrencyHistory>()
                .ForMember(dst => dst.Buy, opt => opt.MapFrom(src => src.Buy))
                .ForMember(dst => dst.Sale, opt => opt.MapFrom(src => src.Sale))
                .ForMember(dst => dst.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dst => dst.Id, opt => opt.Ignore());
        }
    }
}