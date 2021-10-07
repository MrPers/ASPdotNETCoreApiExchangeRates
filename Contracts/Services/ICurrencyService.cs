using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyHistoryDto>> GetWellAsync(string title);
        Task<IEnumerable<CurrencyDto>> GetAll();
        Task<long> RegisterAsync(CurrencyDto currencyDto);
    }
}