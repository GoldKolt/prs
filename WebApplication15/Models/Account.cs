using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; }
        public decimal Debet { get; set; }
        public decimal Credit { get; set; }
        public decimal Saldo { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
