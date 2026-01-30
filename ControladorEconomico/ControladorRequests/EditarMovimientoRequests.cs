using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ControladorRequests
{
    public class EditarMovimientoRequests
    {
        [Required(ErrorMessage = "La categoria es obligatoria"), Range(1, int.MaxValue, ErrorMessage = "Error en el rango")]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "El objetivo es obligatorio"), Range(1, int.MaxValue, ErrorMessage = "Error en el rango")]
        public int? ObjetivoId { get; set; }
        [Required(ErrorMessage = "El monto es obligatorio"), Range(1, double.MaxValue, ErrorMessage = "El monto debe ser positivo")]
        public double Monto { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }
    }
}
