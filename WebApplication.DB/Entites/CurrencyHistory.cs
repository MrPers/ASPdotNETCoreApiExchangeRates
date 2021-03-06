using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.DB.Entites
{
    public class CurrencyHistory : BaseEntity<long>
    {
        public double Buy { get; set; }
        public double Sale { get; set; }
        public DateTime Data { get; set; }
        public long CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
