using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class DepositAccount : Account
    {
        public int DepositContractId { get; set; }
        public DepositContract DepositContract { get; set; }
        public bool IsForPercents { get; set; }
    }
}
