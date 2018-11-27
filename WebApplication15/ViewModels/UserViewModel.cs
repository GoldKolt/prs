using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication15.Models;

namespace WebApplication15.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\-]+$", ErrorMessage = "Use letters only please")]
        public string Surname { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\-]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\-]+$", ErrorMessage = "Use letters only please")]
        public string SecondName { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}")]
        public string BirthDate { get; set; }

        [Required]
        public Sex Sex { get; set; }

        [Required]
        public string IssuedBy { get; set; }

        [Required]
        public string PassportSeries { get; set; }

        [Required]
        public string PassportNumber { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}")]
        public string IssueDate { get; set; }

        [Required]
        public string PassportId { get; set; }

        [Required]
        public string BirthPlace { get; set; }

        [Required]
        public Cities CityOfResidence { get; set; }

        [Required]
        public string AddressOfResidence { get; set; }

        public string HomePhone { get; set; }

        public string MobilePhone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Cities RegistrationCity { get; set; }

        [Required]
        public MaritalStatus MaritalStatus { get; set; }

        [Required]
        public Nationality Nationality { get; set; }

        [Required]
        public DisabilityGroup DisabilityGroup { get; set; }

        [Required]
        public bool Pensioner { get; set; }

        [RegularExpression(@"^[0-9]+(\,[0-9])?[0-9]?$", ErrorMessage = "Use letters only please")]
        public string MonthlyIncome { get; set; }

        public List<SelectListItem> CitiesOfResidence { get; set; }
        public List<SelectListItem> RegistrationCities { get; set; }
        public List<SelectListItem> MaritalStatuses { get; set; }
        public List<SelectListItem> Disabilities { get; set; }
        public List<SelectListItem> Nationalities { get; set; }
        public List<SelectListItem> Sexes { get; set; }
    }
}
