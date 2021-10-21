using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task Add(CurrencyDto currencyDto, bool save =true)
        {
            var currency = _mapper.Map<Currency>(currencyDto);

            await _context.Set<Currency>().AddAsync(currency);
            if (save)
                await _context.SaveChangesAsync();
        }

        public async Task<long> Add(CurrencyHistoryDto currencyHistoryDto, bool save = true)
        {
            var currencyHistory = _mapper.Map<CurrencyHistory>(currencyHistoryDto);
            await _context.Set<CurrencyHistory>().AddAsync(currencyHistory);
            if (save)
                await _context.SaveChangesAsync();
            return currencyHistory.Id;
        }

        public async Task<CurrencyDto> GetByName(string name)
        {
            var currency = await this._context.Currencies.SingleOrDefaultAsync(el => el.Name == name);
            return _mapper.Map<CurrencyDto>(currency);
        }

        public async Task<long> GetCurrencyIdByName(string name)
        {
            return (await _context.Currencies.SingleOrDefaultAsync(el => el.Name == name))?.Id ?? 0;
        }

        public async Task<ICollection<CurrencyHistoryDto>> GetHistory(long currencyId, string scale, DateTime dtStart, DateTime dtFinal)
        {
            var dbItems = await _context.GetCurrrencyHistory(currencyId, scale, dtStart.ToString(), dtFinal.ToString());

            return _mapper.Map<ICollection<CurrencyHistoryDto>>(dbItems);
        }
    }
    
}