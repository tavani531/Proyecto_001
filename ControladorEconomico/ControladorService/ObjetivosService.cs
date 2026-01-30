using ControladorData;
using ControladorEntities;
using ControladorResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControladorService
{
    public class ObjetivosService
    {

        private ApiResponse<List<ObjetivosEntities>> LeerObjetivos()
        {
            var objetivos = Archivos.LeerObjetivosEnJson();
            if (objetivos == null)
            {
                return new ApiResponse<List<ObjetivosEntities>>("Error al leer los objetivos", new List<string> { "Error al leer los objetivos en el json." });
            }
            return new ApiResponse<List<ObjetivosEntities>>(objetivos);
        }
        private ApiResponse<ObjetivosEntities> GuardarObjetivo(ObjetivosEntities objetivo)
        {
            var objetivoGuardado = Archivos.GuardarObjetivoEnJson(objetivo);

            if (objetivoGuardado == null)
            {
                return new ApiResponse<ObjetivosEntities>("No se pudo guardar el objetivo", new List<string> { "No se pudo guardar el objetivo en json" });
            }
            return new ApiResponse<ObjetivosEntities>(objetivoGuardado);
        }
        private ApiResponse<ObjetivosEntities> BuscarObjetivo(int id)
        {
            var objetivos = LeerObjetivos();
            if (!objetivos.Success)
            {
                return new ApiResponse<ObjetivosEntities>("Error inesperado", objetivos.Errors);
            }
            var objetivo = objetivos.Data!.FirstOrDefault(b => b.Id == id);
            if (objetivo == null)
            {
                return new ApiResponse<ObjetivosEntities>("No se encontro el movimiento", objetivos.Errors);
            }
            return new ApiResponse<ObjetivosEntities>(objetivo);
        }
        private bool EiminarObjetivos(int id)
        {
            var objetivo = Archivos.EliminarObjetivo(id);
            return objetivo;
        }
    }
}
