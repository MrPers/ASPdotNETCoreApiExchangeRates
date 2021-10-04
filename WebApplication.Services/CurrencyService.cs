using AutoMapper;
//using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using WebApplication.DTO;
using WebApplication.Models;
using WebApplication.Repository;

namespace WebApplication.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository currencyService, IMapper mapper)
        {
            _currencyRepository = currencyService;
            _mapper = mapper;
        }

        public IEnumerable<CurrencyHistoryDto> GetWellAsync(string title)
        {

            var currencyId = _currencyRepository.GetCurrencyIdByName(title);
            var currencyHistory = _currencyRepository.GetHistory(currencyId);

            return currencyHistory;
        }
    }
}