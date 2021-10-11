using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
            var currencyId = _currencyRepository.GetAll();

            return currencyId;
        }

        public async Task<IEnumerable<CurrencyHistoryDto>> GetWellAsync(string title)
        {
            var currencyId = await _currencyRepository.GetCurrencyIdByName(title);
            var currencyHistory = await _currencyRepository.GetHistory(currencyId);

            return currencyHistory;
        }

        public async Task<long> RegisterAsync(CurrencyDto currencyDto)
        {
            return await _currencyRepository.Add(currencyDto);
        }
        public async Task<long> RegisterAsync(IFormFile file)
        {
            var currencyId = await _currencyRepository.GetCurrencyIdByName(file.FileName.Split('/')[0]);

            using (var sreader = new StreamReader(file.OpenReadStream()))
            {
                string[] headers = sreader.ReadLine().Split(',');
                while (!sreader.EndOfStream)
                {
                    string[] rows = sreader.ReadLine().Split(',');
                    var currencyHistoryDto = new CurrencyHistoryDto
                    {
                        //Buy = double.Parse(rows[0].ToString()),
                        //Sale = double.Parse(rows[0].ToString()),
                        //Data = DateTime.Parse(rows[0].ToString()),
                        CurrencyId = currencyId
                    };

                    //await _currencyRepository.Add(currencyHistoryDto);
                }
            }


            return 5;
        }
    }
}