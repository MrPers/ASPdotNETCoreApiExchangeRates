using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<long> Add(CurrencyDto currencyDto, bool save =true)
        {
            var currency = _mapper.Map<Currency>(currencyDto);
            await _context.Set<Currency>().AddAsync(currency);
            if (save)
                await _context.SaveChangesAsync();
            return currency.Id;
        }

        public async Task<long> Add(CurrencyHistoryDto currencyHistoryDto, bool save = true)
        {
            var currencyHistory = _mapper.Map<CurrencyHistory>(currencyHistoryDto);
            await _context.Set<CurrencyHistory>().AddAsync(currencyHistory);
            if (save)
                await _context.SaveChangesAsync();
            return currencyHistory.Id;
        }

        public async Task<long> GetCurrencyIdByName(string name)
        {
            return (await _context.Currencies.SingleOrDefaultAsync(el => el.Name == name))?.Id ?? 0;
        }

        public async Task<ICollection<CurrencyHistoryDto>> GetHistory(long currencyId)
        {
            var dbItems = _context.CurrencyHistories.Where(el => el.CurrencyId == currencyId);
            return _mapper.Map<ICollection<CurrencyHistoryDto>>(dbItems);
        }
    }
}