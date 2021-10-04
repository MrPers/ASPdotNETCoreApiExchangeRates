using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.DB.Entites
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        //[EmailAddress]
        public string Email { get; set; }
        //[Phone]
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
