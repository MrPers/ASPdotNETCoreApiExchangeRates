using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyHistoryDto>> GetWellAsync(string title);
    }
}