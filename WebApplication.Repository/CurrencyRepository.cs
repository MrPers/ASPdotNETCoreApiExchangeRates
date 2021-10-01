using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DB;
using WebApplication.Entites;
using WebApplication.Models;
//using WebApplication.Models;

namespace WebApplication.Repository
{
    public class CurrencyRepository<T> : ICurrencyRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CurrencyRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<CurrencyHistory> GetAll(long Id)
        {
            List<CurrencyHistory> currencyHistory = new List<CurrencyHistory>();

            var currencies = _context.CurrencyHistories.ToList();

            foreach (CurrencyHistory item in currencies)
                if (item.CurrencyId == Id)
                    currencyHistory.Add(item);

            return currencyHistory;
        }

        public long GetIdCurrency(string title)
        {
            foreach (Currency item in _context.Currencies.ToList())
                if (item.Name == title)
                    return item.Id;
            return 0;
        }
    }
}