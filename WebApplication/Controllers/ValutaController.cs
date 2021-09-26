using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication.Models;

namespace WebAppi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValutaController : ControllerBase
    {

        [HttpGet("{value}")]   //  /api/Valuta/data
        public IActionResult SetFavorite(string value)
        {
            Random random = new Random();
            int minValue, maxValue;
            bool drob = false;
            switch (value)
            {
                case "USD":
                    maxValue = 30;
                    minValue = 25;
                    break;
                case "EUR":
                    maxValue = 36;
                    minValue = 29;
                    break;
                case "RUB":
                    maxValue = 50;
                    minValue = 5;
                drob = true;
                    break;
                default:
                    return NotFound();
            }
            DataChart[] dataChart = new DataChart[random.Next(30,200)];
            DateTime date = DateTime.UtcNow.AddDays(-(dataChart.Length));
            for (int i = 0; i < dataChart.Length; i++)
            {
                dataChart[i] = new DataChart();
                dataChart[i].Label = $"{date.AddDays(i)}";
                dataChart[i].Value = drob ?  ((double)(random.Next(minValue, maxValue))/100) : random.Next(minValue, maxValue);
            }
            return Ok( new object []{ value, dataChart });

        }
    }
}