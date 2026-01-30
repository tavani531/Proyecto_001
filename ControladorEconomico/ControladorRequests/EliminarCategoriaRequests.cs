using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ControladorRequests
{
    public class EliminarCategoriaRequests
    {
        [Required(ErrorMessage = "El Id es obligatorio"), Range(1, int.MaxValue, ErrorMessage = "El Id debe ser positivo")]
        public int Id { get; set; }
    }
}
