using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Entities
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public List<CurrencyHistory> CurrencyHistory{ get; set; }
    }
}
