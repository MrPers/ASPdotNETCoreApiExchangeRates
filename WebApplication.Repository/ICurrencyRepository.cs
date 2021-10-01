using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Entites;
using WebApplication.Models;
//using WebApplication.Models;

namespace WebApplication.Repository
{
    public interface ICurrencyRepository<T> where T : BaseEntity
    {
        List<CurrencyHistory> GetAll(long Id);
        long GetIdCurrency(string title);

    }
}
