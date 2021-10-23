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
            uint localMiddle = finish - start - 1;
            uint step = 0;
            double localMiddleDegree = 0;
            double summ = 0;

            for (uint t = 1; t <= localMiddle; t++)
            {
                step = step + t;
            }

            localMiddleDegree = 1.0 / step;
            step = 0;

            for (uint t = start; t < finish - 1; t++)
            {
                step++;
                summ += (dataCH[t + 1].Buy - dataCH[t].Buy) * (localMiddleDegree * step);
            }

            return summ;
        }

        //private void TrendMaxMin(CurrencyHistoryDto[] dataCH, uint start, uint finish, List<uint> numbers, out uint max, out uint min)
        //{
        //    max = start;
        //    min = start;
        //    for (uint t = start + 1; t < finish; t++)
        //    {
        //        max = dataCH[t].Buy > dataCH[max].Buy ? t : max;
        //        min = dataCH[t].Buy < dataCH[min].Buy ? t : min;
        //    }

        //    if (TrendIdentifying(dataCH, min > max ? max : min, finish + 1) > 0)
        //    {
        //        numbers.Add(min);
        //    }
        //    else
        //    {
        //        numbers.Add(max);
        //    }
        //}

        private uint TrendMin(CurrencyHistoryDto[] dataCH, uint position, uint min)
        {
            return dataCH[position].Buy < dataCH[min].Buy ? position : min;
        }

        private uint TrendMax(CurrencyHistoryDto[] dataCH, uint position, uint max)
        {
            return dataCH[position].Buy > dataCH[max].Buy ? position : max;
        }
        
        private double DataTrendMiddle(CurrencyHistoryDto[] dataCH, uint start, uint finish)
        {
            double summ = 0;
            for (uint i = finish; i <= finish; i++)
            {
                summ += dataCH[i].Buy;
            }
            return summ / (finish - start + 1);
        }

        public async Task<IEnumerable<CurrencyHistoryDto>> StatisticsCurrencyHistory(long currencyId, string scale, DateTime dtStart, DateTime dtFinal)
        {
            var currencyHistory = await _currencyRepository.GetHistory(currencyId, scale, dtStart, dtFinal);
            CurrencyHistoryDto[] dataCH = null;

            if (currencyHistory.Count >= 5)
            {
                dataCH = currencyHistory.ToArray();

                foreach (var item in dataCH)
                {
                    item.Buy = item.Sale;
                }
    
                //uint step = (uint)(dataCH.Length / 10 > 3 ? (dataCH.Length / 10) : 3);
                uint step = (uint)(dataCH.Length / 30 > 3 ? (dataCH.Length / 30) : 3);
                double middleData = 0;
                uint max = 0;
                uint min = 0;
                uint timeCheck = 0;
                double stepTrend;
                double coefficientTilt = 1.29;
                double localMax;
                double localMin;
                double localTrend = 0;
                double trend = 0;
                double localDifference = 0;
                List<uint> numbers = new List<uint>{0};

                //TrendMaxMin(dataCH, 0, step, numbers, out max, out min);
                //middleData = DataTrendMiddle(dataCH, t - (step - 1), t);

                //localMax = dataCH[max].Buy;
                //localMin = dataCH[min].Buy;

                for (uint t = 0; t < dataCH.Length - (step - 1); t += (step - 1)) 
                {
                    //if (6 == t)
                    //{ }
                    trend = localTrend;
                    localTrend = TrendIdentifying(dataCH, min > max ? max : min, t);
                    middleData = DataTrendMiddle(dataCH, t, t + (step - 1));

                    if (localTrend != 0)
                    {
                        localDifference = dataCH[t].Buy - dataCH[t - 1].Buy;

                        if (localTrend > 0)
                        {
                            if (localDifference > 0)
                            {
                                max = TrendMax(dataCH, t, max);
                                //min = TrendMin(dataCH, t, min);
                            }
                            else
                            {
                                var te = (Math.Abs(localDifference) + localTrend) / (Math.Abs(localDifference) < localTrend ? localTrend : Math.Abs(localDifference));
                                if (te > coefficientTilt)
                                {
                                    //if (timeCheck != max)
                                        numbers.Add(max);
                                    localMax = dataCH[max].Buy;
                                    if (localMin < dataCH[min].Buy && trend != 0)//
                                        numbers.Add(min);
                                    max = t;
                                    timeCheck = t;
                                    min = t;
                                }
                                else
                                {
                                    min = TrendMin(dataCH, t, min);
                                }
                            }
                        }
                        else
                        {
                            if (localDifference < 0)
                            {
                                //max = TrendMax(dataCH, t, max);
                                min = TrendMin(dataCH, t, min);
                            }
                            else
                            {
                                var te = (Math.Abs(localTrend) + localDifference) / (localDifference < Math.Abs(localTrend) ? Math.Abs(localTrend) : localDifference);
                                if (te > coefficientTilt)
                                {
                                    //if(timeCheck != min)
                                        numbers.Add(min);
                                    localMin = dataCH[min].Buy;
                                    if (localMax < dataCH[max].Buy && trend != 0)
                                        numbers.Add(max);
                                    max = t;
                                    min = t;
                                    timeCheck = t;
                                }
                                else
                                {
                                    max = TrendMax(dataCH, t, max);
                                }
                            }
                        }
                    }
                    else
                    {
                        if(trend < 0)
                        {
                            max = TrendMax(dataCH, t, max);
                        }
                        else
                        {
                            min = TrendMin(dataCH, t, min);
                        }
                    }
                }

                numbers.Add((uint)dataCH.Length - 1);
                numbers.Sort();
                var numbersDistinct = numbers.Distinct().ToArray();

                for (int t = 0; t < numbersDistinct.Length - 1; t++)
                {
                    stepTrend = (dataCH[numbersDistinct[t + 1]].Buy - dataCH[numbersDistinct[t]].Buy) / (numbersDistinct[t + 1] - numbersDistinct[t]);
                    for (uint i = numbersDistinct[t] + 1; i < numbersDistinct[t + 1]; i++)
                    {
                        dataCH[i].Buy = dataCH[i - 1].Buy + stepTrend;
                    }
                }
            }

            return dataCH;
        }
    }
}