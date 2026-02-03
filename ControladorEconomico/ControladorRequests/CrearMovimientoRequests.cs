using System;
using System.ComponentModel.DataAnnotations;

namespace ControladorRequests
{
    public class CrearMovimientoRequests
    {
        [Required]
        public string Descripcion { get; set; } 

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int CategoriaId { get; set; }
    }
}