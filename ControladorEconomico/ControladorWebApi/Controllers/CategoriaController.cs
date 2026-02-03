using ControladorRequests;
using ControladorResponse;
using ControladorService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace ControladorWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriasService _service;

        public CategoriasController(CategoriasService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CrearCategoria(CrearCategoriaRequests requests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(requests);
            }
            var response = _service.CrearCategoria(requests);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult ObtenerCategorias(string? ordenarPor = null, bool ascendente = true)
        {
            var response = _service.ObtenerCategorias(ordenarPor, ascendente);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("{ID}")]
        public IActionResult ObtenerCategoriaPorId(int id)
        {
            var requests = new BuscarCategoriaPorIdRequests { Id = id };
            if (!TryValidateModel(requests))
            {
                return BadRequest(ModelState);
            }
            var response = _service.ObtenerCategoriaPorId(requests);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("{id}")]
        public IActionResult EditarCategoria(int id, EditarCategoriaRequests request)
        {
            if (request.Id != id)
            {
                return BadRequest(request);
            }

            var resultado = _service.EditarCategoria(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarCategoria(int id)
        {
            var request = new EliminarCategoriaRequests { Id = id };
            var resultado = _service.EliminarCategoria(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }
    }
}
