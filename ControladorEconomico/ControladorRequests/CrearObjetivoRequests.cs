using System;
using System.ComponentModel.DataAnnotations;

namespace ControladorRequests
{
    public class CrearObjetivoRequests
    {
        [Required]
        public string NombreObjetivo { get; set; }

        [Required]
        public decimal MontoMeta { get; set; }

        [Required]
        public DateTime FechaLimite { get; set; }
    }
}