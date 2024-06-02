using APITicket.BLL;
using APITicket.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APITicket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        public IConfiguration configuration;
        private Usuario usuario;
        public UsuarioController(IConfiguration configuration)
        {
            this.configuration = configuration;

            usuario = new(this.configuration);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Listar()
        {

            var respuesta = usuario.Listar();

            return Ok(respuesta);

        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Buscar(int id)
        {

            var respuesta = usuario.Buscar(id);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPost]
        public IActionResult Agregar([FromBody] JsonElement resultado)
        {
            var respuesta = usuario.Agregar(resultado);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Modificar(int id, [FromBody] JsonElement resultado)
        {
            var respuesta = usuario.Modificar(id, resultado);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpGet("rol")]
        public IActionResult ListarRol()
        {

            var respuesta = usuario.ListarRol();

            return Ok(respuesta);

        }
        [Authorize]
        [HttpGet("rol/{id}")]
        public IActionResult BuscarRol(int id)
        {

            var respuesta = usuario.BuscarRol(id);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPost("rol")]
        public IActionResult AgregarRol([FromBody] JsonElement resultado)
        {
            var respuesta = usuario.AgregarRol(resultado);

            return Ok(respuesta);

        }
        [Authorize]
        [HttpPut("rol/{id}")]
        public IActionResult ModificarRol(int id, [FromBody] JsonElement resultado)
        {
            var respuesta = usuario.ModificarRol(id, resultado);

            return Ok(respuesta);

        }

       

    }
}
