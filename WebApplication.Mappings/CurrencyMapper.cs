using AutoMapper;
using System;
using WebApplication.Entites;
using WebApplication.Models;

namespace WebApplication.Mappings
{
    public class CurrencyMapper : Profile
    {
        public CurrencyMapper()
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