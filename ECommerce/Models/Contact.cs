using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string message { get; set; }
    }
}
