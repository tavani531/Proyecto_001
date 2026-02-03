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
    public class MovimientosService
    {
        private readonly ApplicationDbContext _context;

        public MovimientosService(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- CREAR ---
        public ApiResponse<MovimientoResponse> CrearMovimiento(CrearMovimientoRequests requests)
        {
            try
            {
                if (!_context.Categorias.Any(c => c.id == requests.CategoriaId))
                {
                    return new ApiResponse<MovimientoResponse>("La categoría especificada no existe");
                }

                var movimientoDb = new MovimientosEntities
                {
                    CategoriaId = requests.CategoriaId,
                    Monto = requests.Monto,
                    Fecha = requests.Fecha, // Usamos la fecha que viene del front
                    Descripcion = requests.Descripcion
                };

                _context.Movimientos.Add(movimientoDb);
                _context.SaveChanges();

                // CLAVE: Recargamos la categoría asociada para poder devolverla al Frontend
                _context.Entry(movimientoDb).Reference(m => m.Categoria).Load();

                var response = new MovimientoResponse
                {
                    Id = movimientoDb.Id,
                    CategoriaId = movimientoDb.CategoriaId,
                    Monto = movimientoDb.Monto,
                    Fecha = movimientoDb.Fecha,
                    Descripcion = movimientoDb.Descripcion,
                    // Mapeamos la categoría para que el front sepa si es Ingreso/Gasto al instante
                    Categoria = movimientoDb.Categoria != null ? new CategoriaResponse
                    {
                        Id = movimientoDb.Categoria.id,
                        NombreCategoria = movimientoDb.Categoria.NombreCategoria,
                        TipoCategoria = movimientoDb.Categoria.TipoCategoria
                    } : null
                };
                return new ApiResponse<MovimientoResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<MovimientoResponse>("Error al crear movimiento", new List<string> { ex.Message });
            }
        }

        // --- OBTENER TODOS (LISTADO) ---
        public ApiResponse<List<MovimientoResponse>> ObtenerMovimientos(string? ordenarPor = null, bool ascendente = true)
        {
            try
            {
                // CLAVE: .Include(m => m.Categoria) trae los datos de la otra tabla
                var query = _context.Movimientos
                                    .Include(m => m.Categoria)
                                    .AsQueryable();

                if (!string.IsNullOrEmpty(ordenarPor))
                {
                    switch (ordenarPor.ToLower())
                    {
                        case "fecha":
                            query = ascendente ? query.OrderBy(m => m.Fecha) : query.OrderByDescending(m => m.Fecha);
                            break;
                        case "monto":
                            query = ascendente ? query.OrderBy(m => m.Monto) : query.OrderByDescending(m => m.Monto);
                            break;
                        case "descripcion":
                            query = ascendente ? query.OrderBy(m => m.Descripcion) : query.OrderByDescending(m => m.Descripcion);
                            break;
                        default:
                            query = query.OrderByDescending(m => m.Fecha);
                            break;
                    }
                }
                else
                {
                    query = query.OrderByDescending(m => m.Fecha);
                }

                var listado = query.ToList();

                var response = listado.Select(b => new MovimientoResponse
                {
                    Id = b.Id,
                    CategoriaId = b.CategoriaId,
                    Monto = b.Monto,
                    Fecha = b.Fecha,
                    Descripcion = b.Descripcion,
                    // CLAVE: Rellenamos la "Caja" CategoriaResponse
                    Categoria = b.Categoria != null ? new CategoriaResponse
                    {
                        Id = b.Categoria.id,
                        NombreCategoria = b.Categoria.NombreCategoria,
                        TipoCategoria = b.Categoria.TipoCategoria
                    } : null
                }).ToList();

                return new ApiResponse<List<MovimientoResponse>>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<MovimientoResponse>>("Error al obtener movimientos", new List<string> { ex.Message });
            }
        }

        // --- OBTENER POR ID ---
        public ApiResponse<MovimientoResponse> ObtenerMovimientoPorId(BuscarMovimientoPorIdRequests requests)
        {
            try
            {
                var movimiento = _context.Movimientos
                                         .Include(m => m.Categoria) // CLAVE
                                         .FirstOrDefault(b => b.Id == requests.Id);

                if (movimiento == null)
                {
                    return new ApiResponse<MovimientoResponse>("No se encontró el movimiento");
                }

                var response = new MovimientoResponse
                {
                    Id = movimiento.Id,
                    CategoriaId = movimiento.CategoriaId,
                    Monto = movimiento.Monto,
                    Fecha = movimiento.Fecha,
                    Descripcion = movimiento.Descripcion,
                    Categoria = movimiento.Categoria != null ? new CategoriaResponse
                    {
                        Id = movimiento.Categoria.id,
                        NombreCategoria = movimiento.Categoria.NombreCategoria,
                        TipoCategoria = movimiento.Categoria.TipoCategoria
                    } : null
                };
                return new ApiResponse<MovimientoResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<MovimientoResponse>("Error al buscar movimiento", new List<string> { ex.Message });
            }
        }

        // --- EDITAR ---
        public ApiResponse<MovimientoResponse> EditarMovimiento(EditarMovimientoRequests requests)
        {
            try
            {
                var movimientoDb = _context.Movimientos.FirstOrDefault(b => b.Id == requests.Id);

                if (movimientoDb == null)
                {
                    return new ApiResponse<MovimientoResponse>("No existe el movimiento para editar");
                }

                movimientoDb.Monto = requests.Monto;
                movimientoDb.Fecha = requests.Fecha;
                movimientoDb.Descripcion = requests.Descripcion;
                movimientoDb.CategoriaId = requests.CategoriaId; // Actualizamos también la categoría

                _context.SaveChanges();

                // Recargamos la categoría nueva por si cambió
                _context.Entry(movimientoDb).Reference(m => m.Categoria).Load();

                var response = new MovimientoResponse
                {
                    Id = movimientoDb.Id,
                    CategoriaId = movimientoDb.CategoriaId,
                    Monto = movimientoDb.Monto,
                    Fecha = movimientoDb.Fecha,
                    Descripcion = movimientoDb.Descripcion,
                    Categoria = movimientoDb.Categoria != null ? new CategoriaResponse
                    {
                        Id = movimientoDb.Categoria.id,
                        NombreCategoria = movimientoDb.Categoria.NombreCategoria,
                        TipoCategoria = movimientoDb.Categoria.TipoCategoria
                    } : null
                };
                return new ApiResponse<MovimientoResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<MovimientoResponse>("Error al editar movimiento", new List<string> { ex.Message });
            }
        }

        // --- ELIMINAR ---
        public ApiResponse<bool> EliminarMovimiento(EliminarMovimientoRequests requests)
        {
            try
            {
                var movimiento = _context.Movimientos.FirstOrDefault(b => b.Id == requests.Id);

                if (movimiento == null)
                {
                    return new ApiResponse<bool>(false, "No existe el movimiento");
                }

                _context.Movimientos.Remove(movimiento);
                _context.SaveChanges();

                return new ApiResponse<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>("Error al eliminar movimiento", new List<string> { ex.Message });
            }
        }
    }
}