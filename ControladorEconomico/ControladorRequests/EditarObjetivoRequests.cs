using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ControladorRequests
{
    public class EditarObjetivoRequests
    {
        [Required(ErrorMessage ="El nombre es obligatorio.")]
        public string NombreOjetivo { get; set; }
        [Required(ErrorMessage ="El monto es obligatorio"), Range(1,double.MaxValue,ErrorMessage ="El monto debe ser positivo")]
        public double MontoMeta { get; set; }
        [Required(ErrorMessage ="La fecha limite es obligatoria")]
        public DateTime FechaLimite { get; set; }
    }
}
