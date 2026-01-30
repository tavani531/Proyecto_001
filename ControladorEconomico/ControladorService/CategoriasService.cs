using ControladorData;
using ControladorEntities;
using ControladorResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControladorService
{
    public class CategoriasService
    {

        private ApiResponse<List<CategoriasEntities>> LeerCategorias()
        {
            var categorias = Archivos.LeerCategoriasEnJson();
            if (categorias == null)
            {
                return new ApiResponse<List<CategoriasEntities>>("Error al leer las categorias", new List<string> { "Error al leer las categorias en el json." });
            }
            return new ApiResponse<List<CategoriasEntities>>(categorias);
        }
        private ApiResponse<CategoriasEntities> GuardarCategoria(CategoriasEntities categoria)
        {
            var categoriaGuardada = Archivos.GuardarCategoriaEnJson(categoria);

            if (categoriaGuardada == null)
            {
                return new ApiResponse<CategoriasEntities>("No se pudo guardar la categoria", new List<string> { "No se pudo guardar la categoria en json" });
            }
            return new ApiResponse<CategoriasEntities>(categoriaGuardada);
        }
        private ApiResponse<CategoriasEntities> BuscarCategoria(int id)
        {
            var categorias = LeerCategorias();
            if (!categorias.Success)
            {
                return new ApiResponse<CategoriasEntities>("Error inesperado", categorias.Errors);
            }
            var categoria = categorias.Data!.FirstOrDefault(b => b.id == id);
            if (categoria == null)
            {
                return new ApiResponse<CategoriasEntities>("No se encontro la categoria", categorias.Errors);
            }
            return new ApiResponse<CategoriasEntities>(categoria);
        }
        private bool EiminarCategoria(int id)
        {
            var categoria = Archivos.EliminarCategoria(id);
            return categoria;
        }
    }
}
