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
    public class ObjetivosService
    {
        private readonly ApplicationDbContext _context;

        public ObjetivosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApiResponse<ObjetivoResponse> CrearObjetivo(CrearObjetivoRequests requests)
        {
            try
            {
                var objetivoDb = new ObjetivosEntities
                {
                    NombreObjetivo = requests.NombreObjetivo,
                    MontoMeta = requests.MontoMeta,
                    FechaLimite = requests.FechaLimite
                };

                _context.Objetivos.Add(objetivoDb);
                _context.SaveChanges();

                var response = new ObjetivoResponse
                {
                    Id = objetivoDb.Id,
                    NombreObjetivo = objetivoDb.NombreObjetivo,
                    MontoMeta = objetivoDb.MontoMeta,
                    FechaLimite = objetivoDb.FechaLimite
                };
                return new ApiResponse<ObjetivoResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ObjetivoResponse>("Error al crear el objetivo", new List<string> { ex.Message });
            }
        }

        public ApiResponse<List<ObjetivoResponse>> ObtenerObjetivos(string? ordenarPor = null, bool ascendente = true)
        {
            try
            {
                var query = _context.Objetivos.AsQueryable();

                if (!string.IsNullOrEmpty(ordenarPor))
                {
                    switch (ordenarPor.ToLower())
                    {
                        case "nombre":
                            query = ascendente ? query.OrderBy(o => o.NombreObjetivo) : query.OrderByDescending(o => o.NombreObjetivo);
                            break;
                        case "monto":
                            query = ascendente ? query.OrderBy(o => o.MontoMeta) : query.OrderByDescending(o => o.MontoMeta);
                            break;
                        case "fecha":
                            query = ascendente ? query.OrderBy(o => o.FechaLimite) : query.OrderByDescending(o => o.FechaLimite);
                            break;
                        default:
                            query = ascendente ? query.OrderBy(o => o.Id) : query.OrderByDescending(o => o.Id);
                            break;
                    }
                }

                var listado = query.ToList();

                var response = listado.Select(b => new ObjetivoResponse
                {
                    Id = b.Id,
                    NombreObjetivo = b.NombreObjetivo,
                    MontoMeta = b.MontoMeta,
                    FechaLimite = b.FechaLimite
                }).ToList();

                return new ApiResponse<List<ObjetivoResponse>>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ObjetivoResponse>>("Error al obtener objetivos", new List<string> { ex.Message });
            }
        }

        public ApiResponse<ObjetivoResponse> ObtenerObjetivoPorId(BuscarObjetivoPorIdRequests requests)
        {
            try
            {
                var objetivo = _context.Objetivos.FirstOrDefault(b => b.Id == requests.Id);

                if (objetivo == null)
                {
                    return new ApiResponse<ObjetivoResponse>("No se encontró el objetivo");
                }

                var response = new ObjetivoResponse
                {
                    Id = objetivo.Id,
                    NombreObjetivo = objetivo.NombreObjetivo,
                    MontoMeta = objetivo.MontoMeta,
                    FechaLimite = objetivo.FechaLimite
                };
                return new ApiResponse<ObjetivoResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ObjetivoResponse>("Error al buscar objetivo", new List<string> { ex.Message });
            }
        }

        public ApiResponse<ObjetivoResponse> EditarObjetivo(EditarObjetivoRequests requests)
        {
            try
            {
                var objetivoDb = _context.Objetivos.FirstOrDefault(b => b.Id == requests.Id);

                if (objetivoDb == null)
                {
                    return new ApiResponse<ObjetivoResponse>("No se encontró el objetivo para editar");
                }

                objetivoDb.NombreObjetivo = requests.NombreObjetivo;

                objetivoDb.MontoMeta = requests.MontoMeta;
                objetivoDb.FechaLimite = requests.FechaLimite;

                _context.SaveChanges();

                var response = new ObjetivoResponse
                {
                    Id = objetivoDb.Id,
                    NombreObjetivo = objetivoDb.NombreObjetivo,
                    MontoMeta = objetivoDb.MontoMeta,
                    FechaLimite = objetivoDb.FechaLimite
                };
                return new ApiResponse<ObjetivoResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ObjetivoResponse>("Error al editar objetivo", new List<string> { ex.Message });
            }
        }

        public ApiResponse<bool> EliminarObjetivo(EliminarObjetivoRequests requests)
        {
            try
            {
                var objetivo = _context.Objetivos.FirstOrDefault(b => b.Id == requests.Id);

                if (objetivo == null)
                {
                    return new ApiResponse<bool>(false, "No existe el objetivo");
                }

                _context.Objetivos.Remove(objetivo);
                _context.SaveChanges();

                return new ApiResponse<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>("Error al eliminar objetivo", new List<string> { ex.Message });
            }
        }
    }
}