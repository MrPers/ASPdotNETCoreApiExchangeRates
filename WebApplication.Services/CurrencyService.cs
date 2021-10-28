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

        public async Task<IEnumerable<CurrencyHistoryDto>> StatisticsCurrencyHistory(long currencyId, string scale, DateTime dtStart, DateTime dtFinal)
        {
            var currencyHistory = await _currencyRepository.GetHistory(currencyId, scale, dtStart, dtFinal);
            List<CurrencyHistoryDto> dataCH = null;
            int minLengthData = 1;
            double differenceBetweenTrend;

            if (currencyHistory.Count > minLengthData)
            {
                dataCH = currencyHistory.ToList();

                //временное решение
                foreach (var item in dataCH)
                {
                    item.Buy = item.Sale;
                }

                List<Step> storySteps = FindStoryStepData(dataCH);

                (double max, double min) globalAverage = DefineAverage(storySteps);

                Stack<int> addressTrends = DefinesTrend(dataCH, globalAverage);

                //for (int t = 0; t < addressTrends.Count - 1; t++)
                while(addressTrends.Count > 0)
                {
                    var con = addressTrends.Pop();
                    var neCon = addressTrends.Peek();
                    if (addressTrends.Count == 1)
                    {
                        neCon = addressTrends.Pop();
                    }

                    differenceBetweenTrend = (dataCH[con].Buy - dataCH[neCon].Buy) / (con - neCon);
                    for (int i = neCon + 1; i < con; i++)
                    {
                        dataCH[i].Buy = dataCH[i - 1].Buy + differenceBetweenTrend;
                    }
                }

            }

            return dataCH;
        }

        private class Step
        {
            public Step(double trend, int max, int min)
            {
                Trend = trend;
                Max = max;
                Min = min;
            }

            public double Trend { get; }
            public int Max { get; }
            public int Min { get; }
        }

        private List<Step> FindStoryStepData(List<CurrencyHistoryDto> dataCH)
        {
            List<Step> storyStep = new List<Step>();

            for (int i = 0; i < dataCH.Count - 1; i++)
            {
                storyStep.Add(
                    new Step(dataCH[i + 1].Buy - dataCH[i].Buy
                    , dataCH[i].Buy > dataCH[i + 1].Buy ? i : i + 1
                    , dataCH[i].Buy < dataCH[i + 1].Buy ? i : i + 1
                    ));
            }

            return storyStep;
        }

        private (double max, double min) DefineAverage(List<Step> storySteps)
        {
            int max = 0;
            int min = 0;
            double maxHistory = 0;
            double minHistory = 0;

            if (storySteps[0].Trend > 0)
            {
                max++;
            }
            else
            {
                min++;
            }

            for (int t = 0; t < storySteps.Count - 1; t++)
            {
                if (storySteps[t].Trend > 0)
                {
                    maxHistory += storySteps[t].Trend;

                    if (storySteps[t + 1].Trend < 0)
                    {
                        min++;
                    }
                }
                else
                {
                    minHistory += storySteps[t].Trend;

                    if (storySteps[t + 1].Trend > 0)
                    {
                        max++;
                    }
                }
            }

            if (storySteps[storySteps.Count - 1].Trend < 0)
            {
                minHistory += storySteps[storySteps.Count - 1].Trend;
            }
            else
            {
                maxHistory += storySteps[storySteps.Count - 1].Trend;
            }

            maxHistory = maxHistory / max;
            minHistory = minHistory / min;

            return (maxHistory, minHistory);
        }

        private Stack<int> DefinesTrend(List<CurrencyHistoryDto> dataCH, (double max, double min) globalAverage)
        {
            int longTrend = 0;
            Stack<int> address = new Stack<int>();
            address.Push(0);

            for (int t = 1; t < dataCH.Count - 1; t++)
            {
                longTrend++;
                
                if (address.Count > 0)
                {
                    if (Math.Abs(dataCH[t].Buy - dataCH[address.Peek()].Buy) > Math.Abs(dataCH[address.Peek()].Buy - dataCH[t - longTrend].Buy))
                    {
                        longTrend = t - address.Peek();
                    }
                }

                if ((dataCH[t + 1].Buy - dataCH[t].Buy < 0 && 
                    dataCH[t].Buy - dataCH[t - longTrend].Buy > globalAverage.max)
                    || (dataCH[t + 1].Buy - dataCH[t].Buy > 0 && 
                    dataCH[t].Buy - dataCH[t - longTrend].Buy < globalAverage.min
                   ))
                {
                    if(address.Peek() != t - longTrend) address.Push(t - longTrend);
                    longTrend = 0;
                }
            }

            if (dataCH[dataCH.Count - (longTrend + 1)].Buy - dataCH[dataCH.Count - 1].Buy > globalAverage.max 
                || dataCH[dataCH.Count - (longTrend + 1)].Buy - dataCH[dataCH.Count - 1].Buy < globalAverage.min
                )
            {
                address.Push(dataCH.Count - (longTrend + 1));
            }

            address.Push(dataCH.Count - 1);

            return address;
        }
        
        private double FindTrend(List<CurrencyHistoryDto> dataCH, int start, int finish)//  bool flagPositiveTrend = true)
        {
            double summ = 0;

            for (int t = start; t < finish; t++)
            {
                summ += dataCH[t + 1].Buy - dataCH[t].Buy;
            }

            return summ;
        }



        private double DefineTrend(List<CurrencyHistoryDto> dataCH, int start, int finish)
        {
            int localMiddle = finish - start;
            //int step = 0;
            //double localMiddleDegree = 0;
            double summ = 0;

            if (localMiddle > 0)
            {
                //for (int t = 1; t <= localMiddle; t++)
                //{
                //    step = step + t;
                //}

                //localMiddleDegree = 1.0 / step;
                //step = 0;

                //for (int t = start; t < finish; t++)
                //{
                //    step++;
                //    summ += (dataCH[t + 1].Buy - dataCH[t].Buy) * (localMiddleDegree * step);
                //}


                for (int t = start; t < finish; t++)
                {
                    summ += dataCH[t + 1].Buy - dataCH[t].Buy;
                }

            }

            return summ;
        }

        private int TrendMin(List<CurrencyHistoryDto> dataCH, int position, int minDataAddress)
        {
            return dataCH[position].Buy < dataCH[minDataAddress].Buy ? position : minDataAddress;
        }

        private int TrendMax(List<CurrencyHistoryDto> dataCH, int position, int maxDataAddress)
        {
            return dataCH[position].Buy > dataCH[maxDataAddress].Buy ? position : maxDataAddress;
        }


    }
}