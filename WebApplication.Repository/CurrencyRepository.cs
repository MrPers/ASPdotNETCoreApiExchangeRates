using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DB;
using WebApplication.DB.Entites;
using WebApplication.DTO;
using WebApplication.Models;
//using WebApplication.Models;

namespace WebApplication.Repository
{
    public class CurrencyRepository : BaseRepository<Currency, CurrencyDto, long>, ICurrencyRepository
    {
        public CurrencyRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public long GetCurrencyIdByName(string name)
        {
            return this._context.Currencies.SingleOrDefault(el => el.Name == name)?.Id ?? 0;
        }

        public ICollection<CurrencyHistoryDto> GetHistory(long currencyId)
        {
            var dbItems = this._context.CurrencyHistories.Where(el => el.CurrencyId == currencyId);
            return _mapper.Map<ICollection<CurrencyHistoryDto>>(dbItems);
        }
    }
}