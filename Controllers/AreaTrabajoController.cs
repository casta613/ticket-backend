using APITicket.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APITicket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AreaTrabajoController : Controller
    {
        public IConfiguration configuration;
        private AreaTrabajo areaTrabajo;
        public AreaTrabajoController(IConfiguration configuration)
        {
            this.configuration = configuration;

            areaTrabajo = new(this.configuration);
        }
        [Authorize]
        [HttpGet("listar")]
        public IActionResult Listar()
        {

            var respuesta = areaTrabajo.Listar();

            return Ok(respuesta);

        }
        [Authorize]
        [HttpGet("listar/{id}")]
        public IActionResult Buscar(int id)
        {

            var respuesta = areaTrabajo.Buscar(id);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPost]
        public IActionResult Agregar([FromBody] JsonElement resultado)
        {
            var respuesta = areaTrabajo.Agregar(resultado);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPut("modificar/{id}")]
        public IActionResult Modificar(int id, [FromBody] JsonElement resultado)
        {
            var respuesta = areaTrabajo.Modificar(id, resultado);

            return Ok(respuesta);

        }
    }
}
