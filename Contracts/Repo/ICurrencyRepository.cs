using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Repository
{
    public interface ICurrencyRepository : IBaseRepository<CurrencyDto, CurrencyDto, long>
    {
        Task<long> GetCurrencyIdByName(string name);
        Task<ICollection<CurrencyHistoryDto>> GetHistory(long currencyId, string scale, DateTime dtStart, DateTime dtFinal);
        Task Add(CurrencyDto currencyDto, bool save = true);
        Task<long> Add(CurrencyHistoryDto currencyHistoryDto, bool save = true);
        Task<CurrencyDto> GetByName(string name);
    }
}
