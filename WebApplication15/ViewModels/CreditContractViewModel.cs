using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication15.ViewModels
{
    public class CreditContractViewModel
    {
        public int Id { get; set; }

        public int CreditId { get; set; }

        public string Currency { get; set; }

        public string UserName { get; set; }

        public string Number { get; set; }

        [Required]
        public string BeginDate { get; set; }

        [Required]
        public string EndDate { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+(\,[0-9])?[0-9]?$", ErrorMessage = "Wrong value")]
        public string CreditAmount { get; set; }

        public string CreditPercent { get; set; }

        public string Debt { get; set; }

        public int UserId { get; set; }

        public List<SelectListItem> Credits { get; set; }
        public List<SelectListItem> Currencies { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
