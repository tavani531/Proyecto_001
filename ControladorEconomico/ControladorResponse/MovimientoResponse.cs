using System;
using System.Collections.Generic;
using System.Text;

namespace ControladorResponse
{
    public class MovimientoResponse
    {
        public int CategoriaId { get; set; }
        public int? ObjetivoId { get; set; }
        public double Monto { get; set; }
        public string Descripcion { get; set; }
    }
}
