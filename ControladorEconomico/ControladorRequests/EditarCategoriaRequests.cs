using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ControladorRequests
{
    public class EditarCategoriaRequests
    {
        [Required(ErrorMessage = "El Id es obligatorio"), Range(1, int.MaxValue, ErrorMessage = "El Id debe ser positivo")]
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre de la categoria es obligatorio")]
        public string NombreCategoria { get; set; }

        [Required(ErrorMessage = "El tipo de categoria es obligatorio")]
        public bool TipoCategoria { get; set; }
    }
}
