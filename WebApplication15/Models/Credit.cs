using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class Credit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool HasAnnuityPayments { get; set; }

        public decimal Rate { get; set; }

        public int Term { get; set; }
    }
}
