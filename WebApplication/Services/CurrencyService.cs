using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entities;
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

        IEnumerable<Currency> ICurrencyService.GetWellAsync(string title)
        {

            var currency = _currencyRepository.GetIdCurrency(title);

            var currencyHistoy = _currencyRepository.GetAll(currency);

            //var newUser = new UserModelDto
            //{
            //    Id = newUserId,
            //    Name = user.Name,
            //    Password = user.Password
            //};

            return null;
        }
    }
}
//Random random = new Random();
//Money money = new Money();

//switch (value)
//{
//    case "USD":
//        maxValue = 30;
//        minValue = 25;
//        break;
//    case "EUR":
//        maxValue = 36;
//        minValue = 29;
//        break;
//    case "RUB":
//        maxValue = 50;
//        minValue = 5;
//        drob = true;
//        break;
//    default:
//        return null;
//}
//DataChart[] dataChart = new DataChart[random.Next(30, 200)];
//DateTime date = DateTime.UtcNow.AddDays(-(dataChart.Length));
//for (int i = 0; i < dataChart.Length; i++)
//{
//    dataChart[i] = new DataChart();
//    dataChart[i].Label = $"{date.AddDays(i)}";
//    dataChart[i].Value = drob ? ((double)(random.Next(minValue, maxValue)) / 100) : random.Next(minValue, maxValue);
//}
//return dataChart;