using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class CreditAccount : Account
    {
        public int CreditContractId { get; set; }
        public CreditContract CreditContract { get; set; }
        public bool IsForPercents { get; set; }
    }
}
