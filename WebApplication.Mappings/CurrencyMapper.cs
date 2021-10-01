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
            CreateMap<CurrencyHistoryVM, CurrencyHistory>().ReverseMap();

        }
    }
}