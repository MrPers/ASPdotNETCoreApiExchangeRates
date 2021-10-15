using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyHistoryDto>> GetWellAsync(string title, string scale, DateTime dtStart, DateTime dtFinal);
        Task<IEnumerable<CurrencyDto>> GetAll();
        Task<long> RegisterAsync(CurrencyDto currencyDto);
        Task RegisterAsync(IFormFile file);
    }
}