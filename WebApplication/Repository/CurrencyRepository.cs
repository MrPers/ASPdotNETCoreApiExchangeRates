using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DTO;
using WebApplication.Entities;

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

        public List<T> GetAll(CurrencyModelDto currencyH)
        {
            foreach (CurrencyHistory item in _context.CurrencyHistories.Last())
                if (item.Name == currencyH.Id)
                    return _mapper.Map<CurrencyModelDto>(item);
            return null;
            //return _context.Set<T>().ToList();
        }

        public CurrencyModelDto GetIdCurrency(string title)
        {
            foreach (Currency item in _context.Currencies.ToList())
                if(item.Name == title)
                    return _mapper.Map<CurrencyModelDto>(item);
            return null;
        }
    }
}