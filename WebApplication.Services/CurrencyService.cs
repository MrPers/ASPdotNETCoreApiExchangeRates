using AutoMapper;
//using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using WebApplication.Entites;
using WebApplication.Models;
using WebApplication.Repository;

namespace WebApplication.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository<Currency> currencyService, IMapper mapper)
        {
            _currencyRepository = currencyService;
            _mapper = mapper;
        }

        IEnumerable<CurrencyHistory> ICurrencyService.GetWellAsync(string title)
        {

            var currency = _currencyRepository.GetIdCurrency(title);

            var currencyHistoy = _currencyRepository.GetAll(currency);

            return currencyHistoy;
        }
    }
}