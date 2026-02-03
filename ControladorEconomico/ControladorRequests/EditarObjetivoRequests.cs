using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ControladorRequests
{
    public class EditarObjetivoRequests
    {
        [Required(ErrorMessage = "El Id es obligatorio"), Range(1, int.MaxValue, ErrorMessage = "El Id debe ser positivo")]
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es obligatorio.")]
        public string NombreObjetivo { get; set; }
        [Required(ErrorMessage ="El monto es obligatorio")]
        public decimal MontoMeta { get; set; }
        [Required(ErrorMessage ="La fecha limite es obligatoria")]
        public DateTime FechaLimite { get; set; }
    }
}
