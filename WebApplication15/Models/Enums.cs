using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Models
{
    public class Enums
    {
    }

    public enum Sex
    {
        Male = 0, Female = 1
    }

    public enum Cities
    {
        Minsk = 0, Grodno = 1, Gomel = 2, Mogilev = 3, Brest = 4, Vitebsk = 5
    }

    public enum MaritalStatus
    {
        NotMarried = 0, Married = 1, Divorced = 2, Widowed = 3
    }

    public enum Nationality
    {
        Belarus = 0, Russia = 1, Ukraine = 2, Poland = 3, Latvia = 4, Lithuania = 5
    }

    public enum DisabilityGroup
    {
        None = 0, First = 1, Second = 2, Third = 3
    }

    public enum Currency
    {
        BYN = 0, USD = 1
    }
}
