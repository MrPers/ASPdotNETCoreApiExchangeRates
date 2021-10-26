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


        private double DefineTrend(List<CurrencyHistoryDto> dataCH, int start, int finish)
        {
            int localMiddle = finish - start;
            int step = 0;
            double localMiddleDegree = 0;
            double summ = 0;

            if (localMiddle > 0)
            {
                for (int t = 1; t <= localMiddle; t++)
                {
                    step = step + t;
                }

                localMiddleDegree = 1.0 / step;
                step = 0;

                for (int t = start; t < finish; t++)
                {
                    step++;
                    summ += (dataCH[t + 1].Buy - dataCH[t].Buy) * (localMiddleDegree * step);
                }

            }

            return summ;
        }

        private int TrendMin(List<CurrencyHistoryDto> dataCH, int position, int minDataAddress)
        {
            return dataCH[(int)position].Buy < dataCH[(int)minDataAddress].Buy ? position : minDataAddress;
        }

        private int TrendMax(List<CurrencyHistoryDto> dataCH, int position, int maxDataAddress)
        {
            return dataCH[(int)position].Buy > dataCH[(int)maxDataAddress].Buy ? position : maxDataAddress;
        }
        
        private double LocalTrendForceMaxMin(List<CurrencyHistoryDto> dataCH, int start, int finish, out int maxDataAddress, out int minDataAddress)
        {
            maxDataAddress = start;
            minDataAddress = start;

            for (int i = start; i <= finish; i++)
            {
                maxDataAddress = dataCH[i].Buy > dataCH[maxDataAddress].Buy ? i : maxDataAddress;
                minDataAddress = dataCH[i].Buy < dataCH[minDataAddress].Buy ? i : minDataAddress;
            }

            return DefineTrend(dataCH, start, finish);
        }

        private List<int> DefinesTrend(
            List<(double stepTrend, int stepMax, int stepMin, int stepSize, int stepStart)> StoryStep
            ,List<CurrencyHistoryDto> dataCH
            ,double coefficientTilt
            ,List<int> numbers
            )
        {
            List<int> addressTrends = new List<int>();
            double lastTrend = 0;
            double trend = StoryStep[0].stepTrend;
            double trendTilt = 0;
            int lastValue;
            int longLastTrend = 0;
            int maxGlobalAddress = StoryStep[0].stepMax;
            int minGlobalAddress = StoryStep[0].stepMin;
            
            for (int t = 1; t < StoryStep.Count; t++)
            {
                longLastTrend++;

                lastTrend = trend;
                trendTilt =
                    ( Math.Abs(StoryStep[t].stepTrend) + Math.Abs(trend)) / (Math.Abs(trend) < Math.Abs(StoryStep[t].stepTrend)
                    ? Math.Abs(StoryStep[t].stepTrend)
                    : Math.Abs(trend));

                if (trend > 0) 
                {
                    if (StoryStep[t].stepTrend < 0 && trendTilt > coefficientTilt && numbers.IndexOf(minGlobalAddress) == -1) 
                    {
                        addressTrends.Add(minGlobalAddress);
                        longLastTrend = 0;
                    }
                }
                else
                {
                    if (StoryStep[t].stepTrend > 0 && trendTilt > coefficientTilt && numbers.IndexOf(minGlobalAddress) == -1)
                    {
                        addressTrends.Add(maxGlobalAddress);
                        longLastTrend = 0;
                    }
                }

                trend = DefineTrend(dataCH
                    , StoryStep[t - longLastTrend].stepStart
                    , StoryStep[t].stepSize + StoryStep[t].stepStart
                    );

                maxGlobalAddress = TrendMax(dataCH, StoryStep[t].stepMax, StoryStep[t - longLastTrend].stepMax); //maxGlobalAddress);
                minGlobalAddress = TrendMin(dataCH, StoryStep[t].stepMin, StoryStep[t - longLastTrend].stepMin); //minGlobalAddress);
            }

            if (lastTrend > 0)
            {
                lastValue =
                    trend > 0
                    ? StoryStep[StoryStep.Count - 1].stepMax
                    : maxGlobalAddress;
            }
            else
            {
                lastValue =
                    trend < 0
                    ? StoryStep[StoryStep.Count - 1].stepMin
                    : minGlobalAddress;
            }

            addressTrends.Add(lastValue);

            return addressTrends;
        }

        private List<(double stepTrend, int stepMax, int stepMin, int stepSize, int stepStart)> StoryStepData(List<CurrencyHistoryDto> dataCH, int minSeanseSize, int minLengthData)
        {
            int sizeStep = (int)(dataCH.Count / minSeanseSize > minLengthData ? Math.Round((double)(dataCH.Count / minSeanseSize)) : minLengthData);
            int firstStep = (dataCH.Count - (minLengthData  - 1))  % sizeStep;
            int maxLocalDataAddress = 0;
            int minLocalDataAddress = 0; 
            List<(double stepTrend, int stepMax, int stepMin, int stepSize, int stepStart)> StoryStep
                = new List<(double stepTrend, int stepMax, int stepMin, int stepSize, int stepStart)>();

            if (firstStep < minLengthData)
            {
                firstStep = firstStep + minLengthData;
                StoryStep.Add(
                    (LocalTrendForceMaxMin(dataCH, 0, firstStep, out maxLocalDataAddress, out minLocalDataAddress)
                    , maxLocalDataAddress
                    , minLocalDataAddress
                    , firstStep
                    , 0)
                    );
            }

            for (int t = firstStep; t < dataCH.Count - firstStep; t += sizeStep)
            {
                StoryStep.Add(
                    (LocalTrendForceMaxMin(dataCH, t, t + sizeStep, out maxLocalDataAddress, out minLocalDataAddress)
                    , maxLocalDataAddress
                    , minLocalDataAddress
                    , sizeStep
                    , t)
                    );
            }
            
            return StoryStep;
        }

        public async Task<IEnumerable<CurrencyHistoryDto>> StatisticsCurrencyHistory(long currencyId, string scale, DateTime dtStart, DateTime dtFinal)
        {
            var currencyHistory = await _currencyRepository.GetHistory(currencyId, scale, dtStart, dtFinal);
            List<CurrencyHistoryDto> dataCH = null;
            int minLengthData = 2;

            if (currencyHistory.Count > minLengthData)
            {
                dataCH = currencyHistory.ToList();

                //временное решение
                foreach (var item in dataCH)
                {
                    item.Buy = item.Sale;
                }

                int minSeanseSize = 55;
                double coefficientTilt = 1.1;// 1.25;
                List<int> numbers= new List<int> ();
                int maxDataAddress = 0;
                int minDataAddress = 0;
                double localTrend = 0;
                int maxLocalDataAddress = 0;
                int minLocalDataAddress = 0;

                List<(double stepTrend, int stepMax, int stepMin, int stepSize, int stepStart)> StorySteps
                    = StoryStepData(dataCH, minSeanseSize, minLengthData);

                List<int> addressTrends;
                while (true)
                {
                    addressTrends = DefinesTrend(StorySteps, dataCH, coefficientTilt, numbers);
                    addressTrends.Remove(0); 
                    numbers = new List<int>();

                    for (int i = 1; i < addressTrends.Count; i++)
                    {
                        if(addressTrends[i] - addressTrends[i-1] < minLengthData)
                        {
                            numbers.Add(addressTrends[i]);
                            numbers.Add(addressTrends[i - 1]);
                        }
                    }
                    if(numbers.Count == 0)
                    {
                        addressTrends.AddRange(new List<int> { 0, dataCH.Count - 1 });
                        addressTrends.Sort();
                        break;
                    }
                }





                //if (trendTilt > coefficientTilt)
                //{
                //    if (StoryStep[t].stepTrend < 0)
                //    {
                //        addressTrends.Add(minGlobalAddress);
                //    }
                //    else
                //    {
                //        addressTrends.Add(maxGlobalAddress);
                //    }
                //    longLastTrend = 0;
                //}

                for (int t = 0; t < addressTrends.Count - 1; t++)
                {
                    //stepTrend = (dataCH[numbersDistinct[t + 1]].Buy - dataCH[numbersDistinct[t]].Buy) / (numbersDistinct[t + 1] - numbersDistinct[t]);
                    for (int i = addressTrends[t] + 1; i < addressTrends[t + 1]; i++)
                    {
                        dataCH[i].Buy = 1.48;// dataCH[i - 1].Buy + stepTrend;
                    }
                }

            }

            return dataCH;
        }
    }
}