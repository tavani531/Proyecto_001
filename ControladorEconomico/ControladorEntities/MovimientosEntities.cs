using System;
using System.Collections.Generic;
using System.Text;

namespace ControladorEntities
{
    public class MovimientosEntities
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public int? ObjetivoId { get; set; }
        public double Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}
