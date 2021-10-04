using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.DTO
{
    public interface IBaseEntity<T>
    {
        public T Id { get; set; }
    }
}
