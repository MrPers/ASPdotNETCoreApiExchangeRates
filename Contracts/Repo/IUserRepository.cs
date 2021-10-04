using Contracts.Repo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Repository
{
    public interface IUserRepository<T> where T : IBaseEntity
    {
        List<T> GetAll();
        T GetById(long id);
        Task<long> Add(T entity);
    }
}
