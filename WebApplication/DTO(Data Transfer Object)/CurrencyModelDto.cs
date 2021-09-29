using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApplication.DTO
{
    public class CurrencyModelDto
    {
        public long Id { get; set; }
        public string Name { get; set; } 
    }
}
