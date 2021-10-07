using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DB;
using WebApplication.DB.Entites;
using WebApplication.DTO;

namespace WebApplication.Repository
{
    public class CurrencyRepository : BaseRepository<Currency, CurrencyDto, long>, ICurrencyRepository
    {
        public CurrencyRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<long> Add(CurrencyDto currencyDto)
        {
            var currency = _mapper.Map<Currency>(currencyDto);
            await _context.Set<Currency>().AddAsync(currency);
            await _context.SaveChangesAsync();
            return currency.Id;
        }

        public async Task<long> GetCurrencyIdByName(string name)
        {
            return _context.Currencies.SingleOrDefault(el => el.Name == name)?.Id ?? 0;
        }

        public async Task<ICollection<CurrencyHistoryDto>> GetHistory(long currencyId)
        {
            var dbItems = _context.CurrencyHistories.Where(el => el.CurrencyId == currencyId);
            return _mapper.Map<ICollection<CurrencyHistoryDto>>(dbItems);
        }
    }
}