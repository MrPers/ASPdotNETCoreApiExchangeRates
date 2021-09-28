using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entities;

namespace WebApplication.Repository
{
    public interface ICurrencyRepository<T> where T : BaseEntity
    {
        List<T> GetAll(CurrencyModelDto currencyH);
        CurrencyModelDto GetIdCurrency(string title);

    }
}
