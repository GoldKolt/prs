using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime BirthDate { get; set; }

        public Sex Sex { get; set; }

        public string IssuedBy { get; set; }

        public DateTime IssueDate { get; set; }

        public string PassportId { get; set; }

        public string PassportSeries { get; set; }

        public string PassportNumber { get; set; }

        public string BirthPlace { get; set; }

        public Cities CityOfResidence { get; set; }

        public string AddressOfResidence { get; set; }

        public string HomePhone { get; set; }
        
        public string MobilePhone { get; set; }
        
        public string Email { get; set; }

        public Cities RegistrationCity { get; set; }

        public MaritalStatus MaritalStatus { get; set; }
        
        public Nationality Nationality { get; set; }
        
        public DisabilityGroup DisabilityGroup { get; set; }

        public bool Pensioner { get; set; }
        
        public string MonthlyIncome { get; set; }
    }
}
