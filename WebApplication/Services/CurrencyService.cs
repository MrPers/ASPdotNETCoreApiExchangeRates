using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entities;
using WebApplication.Models;
using WebApplication.Repository;

namespace WebApplication.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository<Currency> _currencyRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository<Currency> currencyService, IConfiguration configuration, IMapper mapper)
        {
            _currencyRepository = currencyService;
            _configuration = configuration;
            _mapper = mapper;
        }

        IEnumerable<CurrencyHistoryVM> ICurrencyService.GetWellAsync(string title)
        {

            var currency = _currencyRepository.GetIdCurrency(title);

            var currencyHistoy = _currencyRepository.GetAll(currency);

            return currencyHistoy;
        }
    }
}