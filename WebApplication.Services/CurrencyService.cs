using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        private double TrendIdentifying(CurrencyHistoryDto[] dataCH, uint start, uint finish )
        {
            double diffMaxMin = 0;
            double summ = 0;
            for (uint t = start; t < finish - 1; t++)
            {
                summ += dataCH[t + 1].Buy - dataCH[t].Buy;
            }

            if(summ > 0)
            {
                diffMaxMin = summ / (finish - start - 1);
            }

            return diffMaxMin;
        }

        private void TrendMaxMin(CurrencyHistoryDto[] dataCH, uint start, uint finish, out uint max, out uint min)
        {
            max = start;
            min = start;
            for (uint t = start + 1; t < finish; t++)
            {
                max = dataCH[t].Buy > dataCH[max].Buy ? t : max;
                min = dataCH[t].Buy < dataCH[min].Buy ? t : min;
            }
        }

        private uint TrendMin(CurrencyHistoryDto[] dataCH, uint position, uint min)
        {
            return dataCH[position].Buy < dataCH[min].Buy ? position : min;
        }

        private uint TrendMax(CurrencyHistoryDto[] dataCH, uint position, uint max)
        {
            return dataCH[position].Buy > dataCH[max].Buy ? position : max;
        }

        public async Task<IEnumerable<CurrencyHistoryDto>> StatisticsCurrencyHistory(long currencyId, string scale, DateTime dtStart, DateTime dtFinal)
        {
            var currencyHistory = await _currencyRepository.GetHistory(currencyId, scale, dtStart, dtFinal);
            CurrencyHistoryDto[] dataCH = null;

            if (currencyHistory.Count >= 5)
            {
                dataCH = currencyHistory.ToArray();
                uint step = (uint)(dataCH.Length / 10 > 5 ? (dataCH.Length / 10) : 5);
                List<double> numbersMin = new List<double>();
                List<double> numbersMax = new List<double>();
                List<uint> numbers = new List<uint>();

                double localMax = double.MinValue;
                double localMin = double.MaxValue;
                double localMiddle = double.MinValue;
                double localMiddleMax = double.MinValue;
                double localMiddleMin = double.MaxValue;
                double diffMaxMin;
                double localSupposed;
                uint max = 0;
                uint min = 0;
                double trend = 0;
                double localTrend = 0;

                TrendMaxMin(dataCH, 0, step, out max, out min);

                for (uint t = step; t < dataCH.Length; t++)
                {
                    localTrend = TrendIdentifying(dataCH, max < min ? max : min, t);

                    if(localTrend != 0)
                    {
                        if (localTrend > 0)
                        {
                            double localDifference = dataCH[t].Buy - dataCH[t - 1].Buy;

                            if (localDifference > 0)
                            {
                                //trend = localTrend;
                                max = TrendMax(dataCH, t, max);
                            }
                            else
                            {
                                if (localTrend / (localDifference) > -1)
                                {
                                    //trend = localTrend;
                                }
                                else
                                {
                                    numbers.Add(max);
                                    numbers.Add(min);
                                    max = t;
                                    min = t;

                                }

                            }
                        }
                        else
                        {
                            double localDifference = dataCH[t].Buy - dataCH[t - 1].Buy;

                            if (localDifference < 0)
                            {
                                //trend = localTrend;
                                max = TrendMin(dataCH, t, min);
                            }
                            else
                            {
                                if (localTrend / (localDifference) > -1)
                                {
                                    //trend = localTrend;
                                }
                                else
                                {
                                    numbers.Add(max);
                                    numbers.Add(min);
                                    max = t;
                                    min = t;
                                }

                            }
                        }
                    }
                    
                }

            }

            return currencyHistory;
        }
    }
}