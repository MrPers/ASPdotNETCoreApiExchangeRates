using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entities;
using WebApplication.Models;

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

        public List<CurrencyHistoryVM> GetAll(long Id)
        {
            List<CurrencyHistoryVM> currencyHistory = new List<CurrencyHistoryVM>();

            var currencies = _context.CurrencyHistories.ToList();

            foreach (CurrencyHistory item in currencies)
                if (item.CurrencyId == Id)
                    currencyHistory.Add(_mapper.Map<CurrencyHistory, CurrencyHistoryVM>(item));
            
            return currencyHistory;
        }

        public long GetIdCurrency(string title)
        {
            foreach (Currency item in _context.Currencies.ToList())
                if(item.Name == title)
                    return item.Id;
            return 0;
        }
    }
}