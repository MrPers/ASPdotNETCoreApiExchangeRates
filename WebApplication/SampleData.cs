using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication
{
    public class SampleData
    {
        public static void Initialize(DataContext context)
        {
            context.Database.Migrate();
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
                foreach (var currency in currencies)
                {
                    for (int i = 1; i <= 200; i++)
                    {
                        stepCostcurrencyHistories = random.Next(20 * i, 30 * i);
                        currencyHistoriesTime.Add(
                                new CurrencyHistory
                                    {
                                        Sale = stepCostcurrencyHistories,
                                        Buy = (random.Next(2, 10) + stepCostcurrencyHistories),
                                        Data = DateTime.Now,
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