using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.DB.Entites
{
    public class Currency: BaseEntity<long>
    {
        public string Name { get; set; }

        public virtual List<CurrencyHistory> CurrencyHistory { get; set; }
    }
}
