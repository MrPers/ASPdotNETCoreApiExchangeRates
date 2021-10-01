using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Entites;
using WebApplication.Models;
//using WebApplication.Models;

namespace WebApplication.Services
{
    public interface ICurrencyService
    {
        IEnumerable<CurrencyHistory> GetWellAsync(string title);
    }
}