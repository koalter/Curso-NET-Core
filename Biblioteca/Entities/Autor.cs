using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Biblioteca.Helpers;

namespace Biblioteca.Entities
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(30, ErrorMessage = "El nombre del autor puede tener hasta {1} caracteres")]
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
        //[Range(18, 120)]
        //public int Edad { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre.ToString()[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("Valida Modelo: La primera letra debe ser en mayúscula!", new string[] { nameof(Nombre) });
                }
            }
        }
    }
}
