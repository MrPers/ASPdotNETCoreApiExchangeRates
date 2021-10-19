using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using WebApplication.DTO;
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
            var currencies = _currencyRepository.GetAll();

            return currencies;
        }

        public async Task<CurrencyDto> GetByName(string name)
        {
            var currency = await _currencyRepository.GetByName(name);
            return currency;
        }

        public async Task<IEnumerable<CurrencyHistoryDto>> GetWellAsync(long currencyId, string scale, DateTime dtStart, DateTime dtFinal)
        {
            var currencyHistory = await _currencyRepository.GetHistory(currencyId, scale,  dtStart, dtFinal);

            return currencyHistory;
        }

        public async Task RegisterAsync(CurrencyDto currencyDto)
        {
            await _currencyRepository.Add(currencyDto);
        }

        public async Task RegisterAsync(IFormFile file)
        {
            var currencyId = await _currencyRepository.GetCurrencyIdByName(file.FileName.Split('/')[0]);
            int bacthCount = 5000;
            using (var sreader = new StreamReader(file.OpenReadStream()))
            {
                string[] headers = sreader.ReadLine().Split(',');
                var provider = new CultureInfo("fr-FR");
                var format = "yyyyMMdd HH:mm:ss:fff";
                int counter = 0;
                while (!sreader.EndOfStream)
                {
                    string[] rows = sreader.ReadLine().Split(',');
                    var dateString = rows[0].ToString();
                    var currencyHistoryDto = new CurrencyHistoryDto
                    {
                        Buy = double.Parse(rows[2].ToString()),
                        Sale = double.Parse(rows[1].ToString()),
                        Data = DateTime.ParseExact(dateString, format, provider),
                        CurrencyId = currencyId
                    };
                    await _currencyRepository.Add(currencyHistoryDto,false);
                    
                    if (counter++ == bacthCount)
                    {
                        await _currencyRepository.SaveChanges();
                        counter = 0;
                    }
                }
            }
        }
    }
}