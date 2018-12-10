using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class Card
    {
        public int Id { get; set; }
        public CreditAccount Account { get; set; }
        [RegularExpression(@"[0-9]{12}", ErrorMessage = "Wrong value")]
        public string Number { get; set; }
        [RegularExpression(@"[0-9]{4}", ErrorMessage = "Wrong value")]
        public string Pin { get; set; }
        public decimal Balance { get; set; }
    }
}
