using System.Collections.Generic;
using System.Threading.Tasks; 
using WebApplication.DTO;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface ICurrencyService
    {
        IEnumerable<Currency> GetWellAsync(string title);
    }
}