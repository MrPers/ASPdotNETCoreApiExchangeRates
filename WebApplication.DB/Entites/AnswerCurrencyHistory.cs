using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.DB.Entites
{
    public class AnswerCurrencyHistory
    {
        public DateTime Data { get; set; }
        public double Buy { get; set; }
        public double Sale { get; set; }
    }
}
