using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CurrencyHistoryModel
    {
        public long Id { get; set; }
        public double Buy { get; set; }
        public double Sale { get; set; }
        public DataType Data { get; set; }
    }
}
