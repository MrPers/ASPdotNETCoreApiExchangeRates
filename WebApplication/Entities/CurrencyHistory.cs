using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Entities
{
    public class CurrencyHistory : BaseEntity
    {
        public double Buy { get; set; }
        public double Sale { get; set; }
        public DateTime Data { get; set; }
        public long CurrencyId { get; set; }
        public Currency Currency{ get; set; }
    }
}
