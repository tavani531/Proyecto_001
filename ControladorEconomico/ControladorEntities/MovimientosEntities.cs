using System;
using System.ComponentModel.DataAnnotations.Schema; 

namespace ControladorEntities
{
    public class MovimientosEntities
    {
        public int Id { get; set; }

        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual CategoriasEntities Categoria { get; set; }

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}