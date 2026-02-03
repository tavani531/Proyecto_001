using System;
using System.Collections.Generic;
using System.Text;

namespace ControladorEntities
{
    public class ObjetivosEntities
    {
        public int Id { get; set; }
        public string NombreObjetivo { get; set; }
        public decimal MontoMeta { get; set; }
        public DateTime FechaLimite { get; set; }

    }
}
