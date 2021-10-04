using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public abstract class BaseRepository<TTable,TDto, TId> : IBaseRepository<TTable, TDto, TId> where TTable : class, IBaseEntity<TId>
    {
        protected readonly DataContext _context;
        protected readonly IMapper _mapper;
        public BaseRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TDto ById(TId id)
        {
            var dbItem = _context.Set<TTable>().Find(id);
            return _mapper.Map<TDto>(dbItem);
        }

        public ICollection<TDto> GetAll()
        {
            var dbItems = _context.Set<TTable>().ToList();
            return _mapper.Map<ICollection<TDto>>(dbItems);
        }
    }
}