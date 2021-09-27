using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CurrencyModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
