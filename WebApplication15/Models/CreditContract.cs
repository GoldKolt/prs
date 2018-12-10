using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class CreditContract
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int CreditId { get; set; }
        public Credit Credit { get; set; }
        public decimal CreditAmount { get; set; }
        public Currency Currency { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<CreditAccount> Accounts { get; set; }
    }
}
