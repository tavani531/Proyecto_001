using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ControladorRequests
{
    public class CrearCategoriaRequests
    {
        [Required(ErrorMessage = "El nombre de la categoria es obligatorio")]
        public string NombreCategoria { get; set; }

        [Required(ErrorMessage = "El tipo de categoria es obligatorio")]
        public bool TipoCategoria { get; set; }
    }
}
