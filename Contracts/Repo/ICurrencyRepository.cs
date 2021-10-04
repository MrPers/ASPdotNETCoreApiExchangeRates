using System.Collections.Generic;
using WebApplication.DTO;

namespace WebApplication.Repository
{
    public interface ICurrencyRepository: IBaseRepository<CurrencyDto, CurrencyDto, long>
    {
        long GetCurrencyIdByName(string name);
        ICollection<CurrencyHistoryDto> GetHistory(long currencyId);
    }
}
