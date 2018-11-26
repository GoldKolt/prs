using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication15.ViewModels
{
    public class DepositContractViewModel
    {
        public int Id { get; set; }

        public int DepositId { get; set; }

        public string Currency { get; set; }

        public string UserName { get; set; }

        public string Number { get; set; }

        [Required]
        public string BeginDate { get; set; }

        [Required]
        public string EndDate { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+(\,[0-9])?[0-9]?$", ErrorMessage = "Wrong value")]
        public string DepositAmount { get; set; }

        public string DepositPercent { get; set; }

        public int UserId { get; set; }

        public List<SelectListItem> Deposits { get; set; }
        public List<SelectListItem> Currencies { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
