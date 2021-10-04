using System.Collections.Generic;
using WebApplication.DTO;

namespace WebApplication.Services
{
    public interface ICurrencyService
    {
        IEnumerable<CurrencyHistoryDto> GetWellAsync(string title);
    }
}