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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    Random random = new Random();
        //    int currencyHistoriesTimeId, stepCostcurrencyHistories;

        //    Currency EUR = new Currency { Id = 1, Name = "EUR" };
        //    Currency USD = new Currency { Id = 2, Name = "USD" };
        //    Currency RUB = new Currency { Id = 3, Name = "RUB" };

        //    modelBuilder.Entity<Currency>().HasData(
        //        new Currency[] { EUR, USD, RUB }
        //    );

        //    CurrencyHistory[] currencyHistoriesTime = new CurrencyHistory[600];

        //    for (int curTime = 1; curTime <= 3; curTime++)
        //        for (int i = 1; i <= 200; i++)
        //        {
        //            currencyHistoriesTimeId = ((curTime - 1) * i) + i;
        //            stepCostcurrencyHistories = random.Next(20 * i, 30 * i);
        //            currencyHistoriesTime[currencyHistoriesTimeId - 1] = new CurrencyHistory
        //            { Id = currencyHistoriesTimeId, Sale = stepCostcurrencyHistories, Buy = (random.Next(2, 10) + stepCostcurrencyHistories), Data = DateTime.Now, CurrencyId = curTime };
        //        }

        //    modelBuilder.Entity<CurrencyHistory>().HasData(currencyHistoriesTime);

        //    //DateTime date = DateTime.UtcNow.AddDays(-(600));
        //    //    dataChart[i].Label = $"{date.AddDays(i)}";

        //    //modelBuilder.Entity<CurrencyHistory>().HasData(
        //    //    new CurrencyHistory[]{
        //    //    new CurrencyHistory { Id=1, Sale = 20, Buy = 40, Data = DateTime.Now, CurrencyId = EUR.Id },
        //    //    new CurrencyHistory { Id=2, Sale = 21, Buy = 40, Data = DateTime.Now, CurrencyId = EUR.Id },
        //    //    new CurrencyHistory { Id=3, Sale = 22, Buy = 42, Data = DateTime.Now, CurrencyId = EUR.Id },
        //    //    new CurrencyHistory { Id=4, Sale = 23, Buy = 49, Data = DateTime.Now, CurrencyId = EUR.Id },
        //    //    new CurrencyHistory { Id=5, Sale = 3, Buy = 49, Data = DateTime.Now, CurrencyId = USD.Id },
        //    //    new CurrencyHistory { Id=6, Sale = 23, Buy = 59, Data = DateTime.Now, CurrencyId = USD.Id },
        //    //    });

        //}

        public static void Initialize(DataContext context)
        {
            context.Database.Migrate();
            if (!context.Currencies.Any() | !context.CurrencyHistories.Any())
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