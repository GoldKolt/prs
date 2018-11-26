using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class DepositContract
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int DepositId { get; set; }
        public Deposit Deposit { get; set; }
        public Currency Currency { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<DepositAccount> Accounts { get; set; }
    }
}
