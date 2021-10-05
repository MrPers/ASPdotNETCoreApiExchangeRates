using AutoMapper;
//using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<CurrencyDto>> GetAll()
        {
            var currencyId = _currencyRepository.GetAll();

            return currencyId;
        }

        public async Task<IEnumerable<CurrencyHistoryDto>> GetWellAsync(string title)
        {

            var currencyId = await _currencyRepository.GetCurrencyIdByName(title);
            var currencyHistory = await _currencyRepository.GetHistory(currencyId);

            return currencyHistory;
        }
    }
}