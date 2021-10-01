using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface ICurrencyService
    {
        IEnumerable<CurrencyHistoryVM> GetWellAsync(string title);
    }
}