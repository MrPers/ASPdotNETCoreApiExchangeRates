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

            if (currencyHistory.Count > minLengthData)
            {
                dataCH = currencyHistory.ToList();

                //временное решение
                for (int i = 0; i < dataCH.Count; i++)
                {
                    dataCH[i].Buy = dataCH[i].Sale;
                    dataCH[i].Data = new DateTime(2000 + i, 7, 20);
                }

                (double max, double min) globalAverage = DefineAverage(dataCH);

                List<int> addressTrends = DefineTrend(dataCH);

                addressTrends = OptimizingGraphics(dataCH, addressTrends, globalAverage);

                dataCH = EditingGraph(dataCH, addressTrends);

            }

            return dataCH;
        }

        private (double max, double min) DefineAverage(List<CurrencyHistoryDto> dataCH)
        {
            int max = 0;
            int min = 0;
            double maxHistory = 0;
            double minHistory = 0;

            if (dataCH[1].Buy - dataCH[0].Buy > 0)
            {
                max++;
            }
            else
            {
                min++;
            }

            for (int t = 1; t < dataCH.Count - 1; t++)
            {
                if (dataCH[t - 1].Buy - dataCH[t].Buy > 0)
                {
                    maxHistory += dataCH[t - 1].Buy - dataCH[t].Buy;

                    if (dataCH[t].Buy - dataCH[t+1].Buy < 0)
                    {
                        min++;
                    }
                }
                else
                {
                    minHistory += dataCH[t - 1].Buy - dataCH[t].Buy;

                    if (dataCH[t].Buy - dataCH[t+1].Buy > 0)
                    {
                        max++;
                    }
                }
            }

            if (dataCH[dataCH.Count - 1].Buy - dataCH[dataCH.Count - 2].Buy < 0)
            {
                minHistory += dataCH[dataCH.Count - 1].Buy - dataCH[dataCH.Count - 2].Buy;
            }
            else
            {
                maxHistory += dataCH[dataCH.Count - 1].Buy - dataCH[dataCH.Count - 2].Buy;
            }

            maxHistory = maxHistory / max;
            minHistory = minHistory / min;
            return (maxHistory, minHistory);
        }

        private List<int> DefineTrend(List<CurrencyHistoryDto> dataCH)
        {
            List<int> addressTrends = new List<int> {0};

            for (int t = 1; t < dataCH.Count - 1; t++)
            {
                if ((dataCH[t].Buy - dataCH[t + 1].Buy < 0
                    && dataCH[t - 1].Buy - dataCH[t].Buy > 0)
                    || (dataCH[t].Buy - dataCH[t + 1].Buy > 0
                    && dataCH[t - 1].Buy - dataCH[t].Buy < 0))
                {
                    addressTrends.Add(t);
                }                
            }

            addressTrends.Add(dataCH.Count - 1);

            return addressTrends;
        }

        private List<CurrencyHistoryDto> EditingGraph(List<CurrencyHistoryDto> dataCH, List<int> addressTrends)
        {
            double differenceBetweenTrend;

            for (int t = 0; t < addressTrends.Count - 1; t++)
            {
                differenceBetweenTrend = (dataCH[addressTrends[t + 1]].Buy - dataCH[addressTrends[t]].Buy) / (addressTrends[t + 1] - addressTrends[t]);
                for (int i = addressTrends[t] + 1; i < addressTrends[t + 1]; i++)
                {
                    dataCH[i].Buy = dataCH[i - 1].Buy + differenceBetweenTrend;
                }
            }
            //while (addressTrends.Count > 0)
            //{
            //    var con = addressTrends.Pop();
            //    var neCon = addressTrends.Peek();
            //    if (addressTrends.Count == 1)
            //    {
            //        neCon = addressTrends.Pop();
            //    }

                
            //}

            return dataCH;
        }

        private List<int> OptimizingGraphics(List<CurrencyHistoryDto> dataCH, List<int> addressTrends, (double max, double min) globalAverage)
        {
            bool r = false;

            //все что меньше мах и мин
            double tr;
            for (int i = 0; i < addressTrends.Count - 1; i++)
            {
                //if (addressTrends[i] == 222)
                //{
                //}
                if (r)
                {
                    i--;
                }
                r = false;
                tr = dataCH[addressTrends[i + 1]].Buy - dataCH[addressTrends[i]].Buy;
                if (!(tr > 0 && tr > globalAverage.max
                    || tr < 0 && tr < globalAverage.min))
                {
                    addressTrends.RemoveAt(i);
                    addressTrends.RemoveAt(i);
                    r = true;
                }
            }

            //объединить одинаковые тенденции
            //for (int t = 1; t < addressTrends.Count - 1; t++)
            //{
            //    if (!((dataCH[addressTrends[t]].Buy - dataCH[addressTrends[t + 1]].Buy < 0
            //        && dataCH[addressTrends[t - 1]].Buy - dataCH[addressTrends[t]].Buy > 0)
            //        || (dataCH[addressTrends[t]].Buy - dataCH[addressTrends[t + 1]].Buy > 0
            //        && dataCH[addressTrends[t - 1]].Buy - dataCH[addressTrends[t]].Buy < 0)))
            //    {
            //        addressTrends.RemoveAt(t);
            //    }
            //}

            //средняя тенденция
            //for (int i = 0; i < addressTrends.Count - 3; i++)
            //{
            //    if (addressTrends[i] == 35)
            //    {
            //    }
            //    if (r)
            //    {
            //        i--;
            //    }
            //    r = false;
            //    if(dataCH[addressTrends[i]].Buy > dataCH[addressTrends[i + 2]].Buy && dataCH[addressTrends[i + 1]].Buy > dataCH[addressTrends[i + 3]].Buy
            //        || dataCH[addressTrends[i]].Buy < dataCH[addressTrends[i + 2]].Buy && dataCH[addressTrends[i + 1]].Buy < dataCH[addressTrends[i + 3]].Buy)
            //    {
            //        addressTrends.RemoveAt(i + 1);
            //        addressTrends.RemoveAt(i + 1);
            //        r = true;
            //    }
            //    else
            //    {
            //        if (dataCH[addressTrends[i]].Buy > dataCH[addressTrends[i + 2]].Buy && dataCH[addressTrends[i + 1]].Buy < dataCH[addressTrends[i + 3]].Buy
            //            || dataCH[addressTrends[i]].Buy < dataCH[addressTrends[i + 2]].Buy && dataCH[addressTrends[i + 1]].Buy > dataCH[addressTrends[i + 3]].Buy)
            //        {
            //            addressTrends.RemoveAt(i + 2);
            //            addressTrends.RemoveAt(i + 2);
            //        }
            //        else
            //        {
            //        }
            //    }
            //    }

            return addressTrends;
        }

        private Stack<int> DefinesTrend(List<CurrencyHistoryDto> dataCH, (double max, double min) globalAverage)
        {
            int longTrend = 0;
            Stack<int> address = new Stack<int>();
            address.Push(0);

            for (int t = 1; t < dataCH.Count - 1; t++)
            {

                if (t == 58)
                {

                }

                longTrend++;

                if (address.Count > 1)
                {
                    var fg = address.Pop();
                    if (((dataCH[address.Peek()].Buy - dataCH[fg].Buy > 0
                        && dataCH[address.Peek()].Buy - dataCH[t].Buy > 0)
                    || (dataCH[address.Peek()].Buy - dataCH[fg].Buy < 0
                        && dataCH[address.Peek()].Buy - dataCH[t].Buy < 0))
                    && Math.Abs(dataCH[address.Peek()].Buy - dataCH[fg].Buy) < Math.Abs(dataCH[address.Peek()].Buy - dataCH[t].Buy))
                    {
                        longTrend = 0;
                    }
                    else
                    {
                        address.Push(fg);
                    }


                }

                if (Math.Abs(dataCH[t].Buy - dataCH[address.Peek()].Buy) > Math.Abs(dataCH[address.Peek()].Buy - dataCH[t - longTrend].Buy))
                {
                    longTrend = t - address.Peek();
                }

                if ((dataCH[t + 1].Buy - dataCH[t].Buy < 0 && 
                    dataCH[t].Buy - dataCH[t - longTrend].Buy > globalAverage.max)
                   ||dataCH[t + 1].Buy - dataCH[t].Buy > 0 && 
                    dataCH[t].Buy - dataCH[t - longTrend].Buy < globalAverage.min)
                {
                    if (address.Peek() != t - longTrend)
                    {
                        address.Push(t - longTrend);
                    }

                    longTrend = 0;
                }
            }

            if (dataCH[dataCH.Count - (longTrend + 1)].Buy - dataCH[dataCH.Count - 1].Buy > globalAverage.max 
            || dataCH[dataCH.Count - (longTrend + 1)].Buy - dataCH[dataCH.Count - 1].Buy < globalAverage.min)
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

        private int TrendMax(List<CurrencyHistoryDto> dataCH, int position, int maxDataAddress)
        {
            return dataCH[position].Buy > dataCH[maxDataAddress].Buy ? position : maxDataAddress;
        }


        //private class Step
        //{
        //    public Step(double trend, int max, int min)
        //    {
        //        Trend = trend;
        //        Max = max;
        //        Min = min;
        //    }

        //    public double Trend { get; }
        //    public int Max { get; }
        //    public int Min { get; }
        //}

        //private List<Step> FindStoryStepData(List<CurrencyHistoryDto> dataCH)
        //{
        //    List<Step> storyStep = new List<Step>();

        //    for (int i = 0; i < dataCH.Count - 1; i++)
        //    {
        //        storyStep.Add(
        //            new Step(dataCH[i + 1].Buy - dataCH[i].Buy
        //            , dataCH[i].Buy > dataCH[i + 1].Buy ? i : i + 1
        //            , dataCH[i].Buy < dataCH[i + 1].Buy ? i : i + 1
        //            ));
        //    }

        //    return storyStep;
        //}

    }
}