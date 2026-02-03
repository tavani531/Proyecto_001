using System;
using System.Collections.Generic;
using System.Text;

namespace ControladorResponse
{
    public class ObjetivoResponse
    {
        public int Id { get; set; }
        public string NombreObjetivo { get; set; }
        public decimal MontoMeta { get; set; }
        public DateTime FechaLimite { get; set; }
    }
}
