using ControladorRequests;
using ControladorResponse;
using ControladorService;
using Microsoft.AspNetCore.Mvc;

namespace ControladorWebApi.Controllers
{
    [Route("api/Movimientos")]
    [ApiController]
    public class MovimientosController : ControllerBase // Le puse la 's' al nombre de la clase por convención
    {
        private readonly MovimientosService _service;

        public MovimientosController(MovimientosService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CrearMovimiento(CrearMovimientoRequests requests)
        {
            if (!ModelState.IsValid)
            {
                // CORRECCIÓN: Devolvemos ModelState para ver QUÉ campo falló
                return BadRequest(ModelState);
            }
            var response = _service.CrearMovimiento(requests);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        // CORRECCIÓN IMPORTANTE: Agregamos "= null" y "= true"
        public IActionResult ObtenerMovimientos(string? ordenarPor = null, bool ascendente = true)
        {
            var response = _service.ObtenerMovimientos(ordenarPor, ascendente);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerMovimientosPorId(int id)
        {
            var requests = new BuscarMovimientoPorIdRequests { Id = id };
            // Nota: TryValidateModel a veces da problemas con objetos simples creados al vuelo, 
            // como solo es un ID, podemos llamar directo al servicio.

            var response = _service.ObtenerMovimientoPorId(requests);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult EditarMovimiento(int id, [FromBody] EditarMovimientoRequests request)
        {
            if (request.Id != id)
            {
                return BadRequest(request);
            }

            var resultado = _service.EditarMovimiento(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarMovimiento(int id)
        {
            var request = new EliminarMovimientoRequests { Id = id };
            var resultado = _service.EliminarMovimiento(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }
    }
}