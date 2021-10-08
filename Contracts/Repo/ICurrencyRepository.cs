using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DTO;

namespace WebApplication.Repository
{
    public interface ICurrencyRepository: IBaseRepository<CurrencyDto, CurrencyDto, long>
    {
        Task<long> GetCurrencyIdByName(string name);
        //long GetCurrencyIdByName(string name);
        Task<ICollection<CurrencyHistoryDto>> GetHistory(long currencyId);
        Task<long> Add(CurrencyDto currencyDto);
        Task<long> Add(CurrencyHistoryDto currencyHistoryDto);
    }
}
