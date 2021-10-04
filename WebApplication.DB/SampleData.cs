using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.DB.Entites;

namespace WebApplication.DB
{
    public class SampleData
    {
        public static void Initialize(DataContext context)
        {
            if (!context.Currencies.Any() || !context.CurrencyHistories.Any())
            {
                Random random = new Random();
                int stepCostcurrencyHistories;

                Currency[] currencies = new Currency[] {
                     new Currency{ Name = "EUR" },
                     new Currency{ Name = "USD" },
                     new Currency{ Name = "RUB" },
                };

                context.Currencies.AddRange(currencies);
                context.SaveChanges();

                List<CurrencyHistory> currencyHistoriesTime = new List<CurrencyHistory>();
                DateTime date = DateTime.UtcNow.AddDays(-(currencyHistoriesTime.Count));

                foreach (var currency in currencies)
                {
                    for (int i = 1; i <= 200; i++)
                    {
                        stepCostcurrencyHistories = random.Next((int)(20 * currency.Id), (int)(30 * currency.Id));
                        currencyHistoriesTime.Add(
                            new CurrencyHistory
                            {
                                Sale = stepCostcurrencyHistories,
                                Buy = (random.Next(2, 10) + stepCostcurrencyHistories),
                                Data = date.AddDays(i),
                                CurrencyId = currency.Id
                            }
                            );
                    }
                }
                context.CurrencyHistories.AddRange(currencyHistoriesTime);
                context.SaveChanges();
            }
        }
    }
}