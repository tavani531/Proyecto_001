using System.ComponentModel.DataAnnotations;

namespace ControladorRequests
{
    public class CrearMovimientoRequests
    {
        [Required(ErrorMessage = "La categoria es obligatoria"), Range(1, int.MaxValue, ErrorMessage = "Error en el rango")]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "El objetivo es obligatorio"), Range(1, int.MaxValue, ErrorMessage = "Error en el rango")]
        public int? ObjetivoId { get; set; }
        [Required(ErrorMessage = "El monto es obligatorio"), Range(1, double.MaxValue, ErrorMessage = "El monto debe ser positivo")]
        public double Monto { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }
    }
}
