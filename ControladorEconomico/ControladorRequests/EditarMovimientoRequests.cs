using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ControladorRequests
{
    public class EditarMovimientoRequests
    {
        [Required(ErrorMessage = "El Id es obligatorio"), Range(1, int.MaxValue, ErrorMessage = "El Id debe ser positivo")]
        public int Id { get; set; }
        [Required(ErrorMessage = "La categoria es obligatoria"), Range(1, int.MaxValue, ErrorMessage = "Error en el rango")]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "El objetivo es obligatorio"), Range(1, int.MaxValue, ErrorMessage = "Error en el rango")]
        public decimal Monto { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
