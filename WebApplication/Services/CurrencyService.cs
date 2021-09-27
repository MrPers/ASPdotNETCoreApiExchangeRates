using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Services
{
    public class CurrencyService
        //<T> where T : BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CurrencyService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public DataChart[] GetWell(string value)
        {
            
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