using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Helpers
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    if (value == null || string.IsNullOrEmpty(value.ToString()))
        //    {
        //        return ValidationResult.Success;
        //    }

        //    var primeraLetra = value.ToString()[0].ToString();

        //    if (primeraLetra.Equals(primeraLetra.ToUpper()))
        //    {

        //    }
        //}
    }
}
