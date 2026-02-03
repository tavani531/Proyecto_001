using ControladorRequests;
using ControladorResponse;
using ControladorService;
using Microsoft.AspNetCore.Mvc;

namespace ControladorWebApi.Controllers
{
    [Route("api/Objetivos")]
    [ApiController]
    public class ObjetivosController : ControllerBase
    {
        private readonly ObjetivosService _service;

        public ObjetivosController(ObjetivosService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CrearObjetivo(CrearObjetivoRequests requests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = _service.CrearObjetivo(requests);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        // CORRECCIÓN IMPORTANTE: Agregamos "= null" y "= true"
        public IActionResult ObtenerObjetivos(string? ordenarPor = null, bool ascendente = true)
        {
            var response = _service.ObtenerObjetivos(ordenarPor, ascendente);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerObjetivoPorId(int id)
        {
            var requests = new BuscarObjetivoPorIdRequests { Id = id };
            var response = _service.ObtenerObjetivoPorId(requests);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult EditarObjetivo(int id,[FromBody] EditarObjetivoRequests request)
        {
            if (request.Id != id)
            {
                return BadRequest(request);
            }
            var resultado = _service.EditarObjetivo(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarObjetivo(int id)
        {
            var request = new EliminarObjetivoRequests { Id = id };
            var resultado = _service.EliminarObjetivo(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }
    }
}