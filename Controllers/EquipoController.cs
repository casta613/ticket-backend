using APITicket.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APITicket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EquipoController : Controller
    {
        public IConfiguration configuration;
        private Equipo equipo;
        public EquipoController(IConfiguration configuration)
        {
            this.configuration = configuration;

            equipo = new(this.configuration);
        }
        [Authorize]
        [HttpGet("listar")]
        public IActionResult Listar()
        {

            var respuesta = equipo.Listar();

            return Ok(respuesta);

        }
        
        [Authorize]
        [HttpGet("buscar/{id}")]
        public IActionResult Buscar(int id)
        {

            var respuesta = equipo.Buscar(id);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPost("agregar")]
        public IActionResult Agregar([FromBody] JsonElement resultado)
        {
            var respuesta = equipo.Agregar(resultado);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPut("modificar/{id}")]
        public IActionResult Modificar(int id, [FromBody] JsonElement resultado)
        {
            var respuesta = equipo.Modificar(id, resultado);

            return Ok(respuesta);

        }
    }
}
