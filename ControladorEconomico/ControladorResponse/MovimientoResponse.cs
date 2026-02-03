using System;
using System.Collections.Generic;
using System.Text;

namespace ControladorResponse
{
    public class MovimientoResponse
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }

        public CategoriaResponse Categoria { get; set; } 

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}