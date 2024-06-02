using APITicket.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APITicket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlantillaTicketController : Controller
    {
        public IConfiguration configuration;
        private PlantillaTicket plantillaTicket;
        public PlantillaTicketController(IConfiguration configuration)
        {
            this.configuration = configuration;

            plantillaTicket = new(this.configuration);
        }
        [Authorize]
        [HttpGet("listar")]
        public IActionResult Listar()
        {

            var respuesta = plantillaTicket.Listar();

            return Ok(respuesta);

        }
        
        [Authorize]
        [HttpGet("buscar/{id}")]
        public IActionResult Buscar(int id)
        {

            var respuesta = plantillaTicket.Buscar(id);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPost("agregar")]
        public IActionResult Agregar([FromBody] JsonElement resultado)
        {
            var respuesta = plantillaTicket.Agregar(resultado);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPut("modificar/{id}")]
        public IActionResult Modificar(int id, [FromBody] JsonElement resultado)
        {
            var respuesta = plantillaTicket.Modificar(id, resultado);

            return Ok(respuesta);

        }
    }
}
