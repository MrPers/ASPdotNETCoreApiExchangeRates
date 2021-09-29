using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public interface ICurrencyRepository<T> where T : BaseEntity
    {
        List<CurrencyHistoryVM> GetAll(long Id);
        long GetIdCurrency(string title);

    }
}
