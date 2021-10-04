using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.DTO
{
    public class CurrencyDto : IBaseEntity<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
