
using System.Collections.Generic;
using WebApplication.DTO;

namespace WebApplication.Repository
{
    public interface IBaseRepository<TTable, TDto, TId> where TTable : IBaseEntity<TId>
    {
        ICollection<TDto> GetAll();
        TDto ById(TId Id);
    }
}
