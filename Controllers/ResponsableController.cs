using APITicket.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APITicket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponsableController : Controller
    {
        public IConfiguration configuration;
        private Responsable responsable;
        public ResponsableController(IConfiguration configuration)
        {
            this.configuration = configuration;

            responsable = new(this.configuration);
        }
        [Authorize]
        [HttpGet("listar")]
        public IActionResult Listar()
        {

            var respuesta = responsable.Listar();

            return Ok(respuesta);

        }
       
        [Authorize]
        [HttpGet("buscar/{id}")]
        public IActionResult Buscar(int id)
        {

            var respuesta = responsable.Buscar(id);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPost("agregar")]
        public IActionResult Agregar([FromBody] JsonElement resultado)
        {
            var respuesta = responsable.Agregar(resultado);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPut("modificar/{id}")]
        public IActionResult Modificar(int id, [FromBody] JsonElement resultado)
        {
            var respuesta = responsable.Modificar(id, resultado);

            return Ok(respuesta);

        }
    }
}
