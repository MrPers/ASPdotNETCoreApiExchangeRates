using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.DTO
{
    public class CurrencyHistoyModelDto
    {
        public long Id { get; set; }
        public double Buy { get; set; }
        public double Sale { get; set; }
        public DateTime Data { get; set; }
        public long CurrencyId { get; set; }
        public CurrencyModelDto CurrencyModelDto { get; set; }
    }
}
