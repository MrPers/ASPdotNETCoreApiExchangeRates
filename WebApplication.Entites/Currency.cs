using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Entites
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public virtual List<CurrencyHistory> CurrencyHistory { get; set; }
    }
}
