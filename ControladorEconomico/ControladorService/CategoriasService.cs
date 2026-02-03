using ControladorData;
using ControladorEntities;
using ControladorRequests;
using ControladorResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControladorService
{
    public class CategoriasService
    {
        private readonly ApplicationDbContext _context;

        public CategoriasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApiResponse<CategoriaResponse> CrearCategoria(CrearCategoriaRequests requests)
        {
            try
            {
                if (_context.Categorias.Any(b => b.NombreCategoria == requests.NombreCategoria))
                {
                    return new ApiResponse<CategoriaResponse>("Esa categoria ya existe");
                }

                var categoriaDb = new CategoriasEntities
                {
                    NombreCategoria = requests.NombreCategoria,
                    TipoCategoria = requests.TipoCategoria
                };

                _context.Categorias.Add(categoriaDb); 
                _context.SaveChanges();             

                var response = new CategoriaResponse
                {
                    Id = categoriaDb.id,
                    NombreCategoria = categoriaDb.NombreCategoria,
                    TipoCategoria = categoriaDb.TipoCategoria
                };
                return new ApiResponse<CategoriaResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoriaResponse>("Error al crear la categoría", new List<string> { ex.Message });
            }
        }
        public ApiResponse<List<CategoriaResponse>> ObtenerCategorias(string? ordenarPor = null, bool ascendente = true)
        {
            try
            {
                // 1. Preparamos la consulta
                var query = _context.Categorias.AsQueryable();

                // 2. Aplicamos el ordenamiento si nos lo piden
                if (!string.IsNullOrEmpty(ordenarPor))
                {
                    switch (ordenarPor.ToLower())
                    {
                        case "nombre":
                            query = ascendente ? query.OrderBy(c => c.NombreCategoria) : query.OrderByDescending(c => c.NombreCategoria);
                            break;
                        case "tipo":
                            query = ascendente ? query.OrderBy(c => c.TipoCategoria) : query.OrderByDescending(c => c.TipoCategoria);
                            break;
                        default:
                            // Por defecto ordenamos por ID
                            query = ascendente ? query.OrderBy(c => c.id) : query.OrderByDescending(c => c.id);
                            break;
                    }
                }
                else
                {
                    // Si no mandan nada, orden normal por ID
                    query = query.OrderBy(c => c.id);
                }

                // 3. Ejecutamos la consulta
                var listado = query.ToList();

                // 4. Mapeamos la respuesta
                var response = listado.Select(c => new CategoriaResponse
                {
                    Id = c.id,
                    NombreCategoria = c.NombreCategoria,
                    TipoCategoria = c.TipoCategoria
                }).ToList();

                return new ApiResponse<List<CategoriaResponse>>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CategoriaResponse>>("Error al obtener categorías", new List<string> { ex.Message });
            }
        }
        public ApiResponse<CategoriaResponse> ObtenerCategoriaPorId(BuscarCategoriaPorIdRequests requests)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(b => b.id == requests.Id);

                if (categoria == null)
                {
                    return new ApiResponse<CategoriaResponse>("No se encontró la categoría");
                }

                var response = new CategoriaResponse
                {
                    Id = categoria.id,
                    NombreCategoria = categoria.NombreCategoria,
                    TipoCategoria = categoria.TipoCategoria,
                };
                return new ApiResponse<CategoriaResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoriaResponse>("Error al buscar categoría", new List<string> { ex.Message });
            }
        }
        public ApiResponse<CategoriaResponse> EditarCategoria(EditarCategoriaRequests requests)
        {
            try
            {
                var categoriaDb = _context.Categorias.FirstOrDefault(b => b.id == requests.Id);

                if (categoriaDb == null)
                {
                    return new ApiResponse<CategoriaResponse>("No se encontró la categoría para editar");
                }

                categoriaDb.NombreCategoria = requests.NombreCategoria;
                categoriaDb.TipoCategoria = requests.TipoCategoria;

                _context.SaveChanges();

                var response = new CategoriaResponse
                {
                    Id = categoriaDb.id,
                    NombreCategoria = categoriaDb.NombreCategoria,
                    TipoCategoria = categoriaDb.TipoCategoria
                };
                return new ApiResponse<CategoriaResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoriaResponse>("Error al editar categoría", new List<string> { ex.Message });
            }
        }
        public ApiResponse<bool> EliminarCategoria(EliminarCategoriaRequests requests)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(b => b.id == requests.Id);
                if (categoria == null)
                {
                    return new ApiResponse<bool>(false, "No existe la categoría");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return new ApiResponse<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>("Error al eliminar (¿Quizás tiene movimientos asociados?)", new List<string> { ex.Message });
            }
        }
    }
}