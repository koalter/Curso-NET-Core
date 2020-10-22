using Biblioteca.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public class AutorDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        //public List<LibroDto> Libros { get; set; }
    }
}
