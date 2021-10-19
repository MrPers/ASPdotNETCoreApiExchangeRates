using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyHistoryDto>> GetWellAsync(long currencyId, string scale, DateTime dtStart, DateTime dtFinal);
        Task<IEnumerable<CurrencyDto>> GetAll();
        Task<CurrencyDto> GetByName(string name);
        Task RegisterAsync(CurrencyDto currencyDto);
        Task RegisterAsync(IFormFile file);
    }
}