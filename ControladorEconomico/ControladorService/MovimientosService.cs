using ControladorData;
using ControladorEntities;
using ControladorResponse;
using System.Security.Cryptography.X509Certificates;

namespace ControladorService
{
    public class MovimientosService
    {

        private ApiResponse<List<MovimientosEntities>> LeerMovimientos()
        {
            var movimientos=Archivos.LeerMovimientosEnJson();
            if (movimientos == null)
            {
                return new ApiResponse<List<MovimientosEntities>>("Error al leer los movimientos", new List<string> { "Error al leer los movimientos en el json." });
            }
            return new ApiResponse<List<MovimientosEntities>>(movimientos);
        }
        private ApiResponse<MovimientosEntities> GuardarMovimiento(MovimientosEntities movimiento)
        {
            var movimientoGuardado=Archivos.GuardarMovimientoEnJson(movimiento);

            if(movimientoGuardado == null)
            {
                return new ApiResponse<MovimientosEntities>("No se pudo guardar el movimiento", new List<string> { "No se pudo guardar el movimiento en json" });
            }
            return new ApiResponse<MovimientosEntities>(movimientoGuardado);
        }
        private ApiResponse<MovimientosEntities> BuscarMovimiento(int id)
        {
            var movimientos = LeerMovimientos();
            if (!movimientos.Success)
            {
                return new ApiResponse<MovimientosEntities>("Error inesperado", movimientos.Errors);
            }
            var movimiento=movimientos.Data!.FirstOrDefault(b=>b.Id==id);
            if( movimiento == null)
            {
                return new ApiResponse<MovimientosEntities>("No se encontro el movimiento", movimientos.Errors);
            }
            return new ApiResponse<MovimientosEntities>(movimiento);
        }
        private bool EiminarMovimientos(int id)
        {
            var movimiento = Archivos.EliminarMovimiento(id);
            return movimiento;
        }
    }
}
