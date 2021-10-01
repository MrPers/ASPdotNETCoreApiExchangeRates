using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Entites;

namespace WebApplication.Repository
{
    public interface IUserRepository<T> where T : BaseEntity
    {
        List<T> GetAll();
        T GetById(long id);
        Task<long> Add(T entity);
    }
}
